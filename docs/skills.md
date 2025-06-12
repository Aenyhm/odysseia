# Synthèse des compétences

## Temps consacré au projet

Le développement du jeu m'a pris 6,5 semaines (environ 1000 heures).

## Ce que j'ai appris

En choisissant d'utiliser l'approche data-oriented design, j'ai appris beaucoup de choses sur le fonctionnement du CPU : comment il stocke les données en mémoire, où il va les chercher et quelles techniques utiliser pour ne pas perdre des cycles.

## Difficultés rencontrées

### Unity

Unity est trèèèès long à charger un projet et à se mettre à jour à chaque modification de code.
Ça n'a l'air de rien comme ça, mais cumulé ça fait perdre pas mal de temps et ça casse un peu le rythme de développement.

Concernant les champs sérialisés (`[SerializeField]`), il faut faire attention car si on change le type ou le nommage dans le code, on perd les valeurs qu'on avait dans l'éditeur.

### Architecture des entités

Je me suis beaucoup posé la question sur la meilleure façon de stocker mes entités, car m'étant mis une contrainte sur la gestion efficace de la mémoire, il m'apparaissait évident que je ne pouvais pas les stocker dans des classes mais dans des structs, pour que, rangées dans un tableau, elles soient contigües en mémoire pour éviter des cache-miss.

#### Héritage ?

Le problème c'est que C# ne supporte pas l'héritage de struct et n'a pas de syntaxe pour le simuler.

#### Composition ?

J'aurais pu utiliser de la composition : chaque entité aurait eu sa propre struct avec un champ pointant vers une struct Entity.

Le problème avec cette approche c'est que je ne pouvais pas ranger mes entités dans un même tableau car elles n'auraient pas eu le même type.

#### Interface ?

J'aurais pu passer par une interface, mais je ne trouvais pas pratique le fait d'appeler `.Entity.Position` par exemple sur chaque entité au lieu de juste `.Position`.

#### Union ?

L'union est l'approche sur laquelle je suis parti car elle permet d'avoir un seul type Entity avec toutes les données nécessaires dedans, et pour chaque type d'entité (via le champ `Type`) on peut connaitre quels champs sont remplis.

```cs
if (e.Type == EntityType.Mermaid) {
    if (boat.CharmedById == e.Id) {
        // Désenvoûte une fois le temps de stun passé.
        e.MermaidData.StunCooldown = Math.Max(0, e.MermaidData.StunCooldown - Clock.DeltaTime);
        if (e.MermaidData.StunCooldown == 0) {
            boat.CharmedById = 0;
        }
    }
}
```

Ça fonctionne mais le seul bémol est que le concept d'union n'existe pas en C#. Donc chaque entité est plus grosse que prévu en mémoire car elle prend de la place pour chaque donnée spécifique à un type d'entité. Mais vu le projet, ça reste totalement anecdotique en termes d'espace mémoire et de performances.

## Améliorations possibles

### Utilisation des events

J'aurais pu utiliser davantage le système d'évènements natif de C# pour découpler chaque mécanique de jeu.
Par exemple : je modifie le score de la partie dans le fichier `Boat.cs` alors que je pourrais lancer un évènement et ce serait dans le fichier `Score.cs` que la modification se ferait.

### Gestion des collisions

Je fais des boucles dans plusieurs fichiers pour gérer les collisions et je pense que ce serait peut-être mieux de les regrouper à un seul endroit.

### UI décisionnaire

Dans mon projet, c'est la partie métier qui est décisionnaire. C'est-à-dire que tout passe par elle avant de donner l'état de jeu à la partie rendu.

Après expérimentation, c'est une technique qui met pas mal de barrières je trouve et je tenterai l'approche inverse où c'est la partie UI qui demande à la partie mécaniques de jeu ce dont elle a besoin. Le découpage resterait inchangé, mais la partie Core exposerait une API à la partie rendu.
