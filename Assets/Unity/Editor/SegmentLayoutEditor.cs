#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Sources;
using Sources.Core;
using Sources.Toolbox;
using Unity.Collections;
using Unity.Scripts;
using UnityEngine;
using UnityEditor;

namespace Unity.Editor {
    
    [CustomEditor(typeof(SegmentScriptable))]
    public class SegmentLayoutEditor : UnityEditor.Editor {
        private const string _iconsFolder = "Assets/Unity/Editor/Icons/";
        private static readonly int _width = Enums.Count<LaneType>();
        private const int _cellSize = 40;
        private SegmentScriptable _scriptable;
        private SegmentScriptable _cleanCopy;
        private string _assetPath;
        private int _height;

        private EntityType selectedType = EntityType.Rock;
        
        private void OnEnable() {
            _scriptable = (SegmentScriptable)target;
            _assetPath = AssetDatabase.GetAssetPath(_scriptable);
            
            if (_scriptable.Segment.EntityCells == null) {
                _scriptable.Segment.Length = SegmentLength.L100;
                _scriptable.Segment.EntityCells = new List<EntityCell>();
            }
            
            _cleanCopy = Instantiate(AssetDatabase.LoadAssetAtPath<SegmentScriptable>(_assetPath));
            _cleanCopy.name = _scriptable.name;
            
            _height = (int)_scriptable.Segment.Length/CoreConfig.GridScale;
        }
        
        private void OnDisable() {
            Restore();
        }

        public override void OnInspectorGUI() {
            var bgColor = GUI.backgroundColor;
            GUI.backgroundColor = ViewConfig.regionThemesByType[_scriptable.Segment.RegionType].WaterColor.ToUnityColor();
            
            var regionType = (RegionType)EditorGUILayout.EnumPopup("Region", _scriptable.Segment.RegionType);
            if (regionType != _scriptable.Segment.RegionType) {
                _scriptable.Segment.RegionType = regionType;
                _scriptable.Segment.EntityCells.Clear();
            }
            
            GUI.backgroundColor = bgColor;

            var enumContent = new GUIContent();
            enumContent.text = "Entity Type";
            
            selectedType = (EntityType)EditorGUILayout.EnumPopup(enumContent, selectedType, AvailableEntityTypes, false);

            EditorGUILayout.Space();
            
            var grid = new SimpleGrid<EntityType>(_width, _height);

            foreach (var e in _scriptable.Segment.EntityCells) {
                var size = EntityLogic.GetEntityTypeDimension(e.Type);
                for (var dx = 0; dx < size.X; dx++) {
                    for (var dy = 0; dy < size.Y; dy++) {
                        var index = grid.CoordsToIndex(e.X + dx, e.Y + dy);
                        grid.Items[index] = e.Type;
                    }
                }
            }

            for (var y = _height - 1; y >= 0; y--) {
                EditorGUILayout.BeginHorizontal();
                
                for (var x = 0; x < _width; x++) {
                    var cellRect = GUILayoutUtility.GetRect(_cellSize, _cellSize);
                    
                    // On laisse 10m de chaque côté de libre pour ne pas créer des tronçons qui se bloquent.
                    if (y == 0 || y == _height - 1) {
                        GUI.DrawTexture(cellRect, LoadIcon("blocked"), ScaleMode.ScaleToFit);
                    } else {
                        var index = grid.CoordsToIndex(x, y);
                        var type = grid.Items[index];
                        var icon = LoadIcon(type.ToString());
                        
                        GUI.DrawTexture(cellRect, icon ?? Texture2D.grayTexture, ScaleMode.ScaleToFit);
                        
                        var e = Event.current;
                        if (e.type == EventType.MouseDown && cellRect.Contains(e.mousePosition)) {
                            Undo.RecordObject(_scriptable, "Change Cell");

                            switch (e.button) {
                                case 0: TryPlace(x, y);  break; // clic gauche
                                case 1: TryRemove(x, y); break; // clic droit
                            }

                            EditorUtility.SetDirty(_scriptable);
                            e.Use(); // Empêche les autres handlers d'utiliser l'événement
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Sauvegarder")) Save();
            
            if (GUILayout.Button("Restaurer")) {
                Undo.RecordObject(_scriptable, "Restore Segment");
                Restore();
                EditorUtility.SetDirty(_scriptable);
                Repaint();
            }
            EditorGUILayout.EndHorizontal();
        }
        
        private void Save() {
            EditorUtility.SetDirty(_scriptable);
            AssetDatabase.SaveAssets();
            EditorUtility.CopySerialized(_scriptable, _cleanCopy);
        }
        
        private void Restore() {
            EditorUtility.CopySerialized(_cleanCopy, _scriptable);
        }
        
        private bool AvailableEntityTypes(Enum value) {
            return EntityLogic.IsEntityTypeAvailableForRegion((EntityType)value, _scriptable.Segment.RegionType);
        }

        private static Texture LoadIcon(string iconName) {
            return (Texture)AssetDatabase.LoadAssetAtPath($"{_iconsFolder}{iconName}.png", typeof(Texture));
        }
        
        private void TryPlace(int x, int y) {
            var size = EntityLogic.GetEntityTypeDimension(selectedType);
            if (selectedType == EntityType.None) return;
            
            if (x + size.X > _width || y + size.Y > _height) return; // Hors limites

            // Vérifie si toute la zone à occuper est libre
            for (var dx = 0; dx < size.X; dx++) {
                for (var dy = 0; dy < size.Y; dy++) {
                    var checkX = x + dx;
                    var checkY = y + dy;

                    foreach (var e in _scriptable.Segment.EntityCells) {
                        var otherSize = EntityLogic.GetEntityTypeDimension(e.Type);
                        for (var ox = 0; ox < otherSize.X; ox++) {
                            for (var oy = 0; oy < otherSize.Y; oy++) {
                                if (e.X + ox == checkX && e.Y + oy == checkY)
                                    return; // collision détectée
                            }
                        }
                    }
                }
            }

            _scriptable.Segment.EntityCells.Add(new EntityCell { Type = selectedType, X = x, Y = y });
        }

        private void TryRemove(int x, int y) {
            for (var i = 0; i < _scriptable.Segment.EntityCells.Count; i++) {
                var e = _scriptable.Segment.EntityCells[i];
                var size = EntityLogic.GetEntityTypeDimension(e.Type);
                
                for (var dx = 0; dx < size.X; dx++) {
                    for (var dy = 0; dy < size.Y; dy++) {
                        if (e.X + dx == x && e.Y + dy == y) {
                            _scriptable.Segment.EntityCells.RemoveAtSwapBack(i);
                            return;
                        }
                    }
                }
            }
        }
    }
}
#endif