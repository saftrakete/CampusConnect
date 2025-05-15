# CampusConnect

Dies ist das Repository für das Softwareprojekt.

## Hinweis zum Erstellen der Datenbank:
 1. Projekt in Visual Studio öffnen
 2. Paket-Manager-Konsole aufrufen
      -  sollte diese nicht bereits unten mit angezeigt werden, dann
         oben in der Leiste auf "Ansicht" -> "Weitere Fenster" -> "Paket-Manager-Konsole"
 3. $ add-migration "name" (z.B. add-migration init)
 4. $ update-database
 5. Die Datenbank sollte erstellt werden, über "Ansicht" kann der SQL-Server-Objekt-Explorer geöffnet werden

## Projekt ausführen
 $ ng serve
