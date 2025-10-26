# PE1 - LINQ, EVENTS EN STREAMS
## ALGEMEEN
### INDIVIDUELE OPDRACHT
Dit is een individuele opdracht. Jouw ingave is een zelfstandig geproduceerd werk waarmee je bewijst dat je over de vereiste vaardigheden beschikt. Gebruik code van derden enkel als studiemateriaal. Deel je oplossingen nooit met anderen. Gekopieerde en gedeelde code resulteert in een nulscore voor de volledige opdracht. Mocht je toch code gebruiken van online bronnen, maak dan gebruik van referenties. Je moet op elk moment kunnen aantonen van waar je de code hebt gehaald.

### AI GEBRUIK
AI-tools zijn voor deze opdracht niet toegestaan. Het gebruik van AI-tools kan leiden tot een nulscore voor de volledige opdracht, en het opstarten van een dossier.
![AI-tools](opgave/image.png)

### GIT EN CONVENTIES
Je werk wordt geëvalueerd op de gebruikelijke Git en code conventies. 
Hanteer een consistente programmeerstijl en maak passende commits bij het bouwen, wijziging of verwijderen van een onderdeel in het project. Er worden minstens tien nuttige commits verwacht. Indien dit niet het geval is, kan dit leiden tot een lagere score.

### INDIENEN
- Maak jouw online repository aan via de Classroom link op Leho.
- Dien jouw oplossing in via GitHub Classroom voor het einde van de afgesproken deadline. 

## OPGAVE
Maak een applicatie die het csv-bestand `stores_products.csv` inleest en de gevraagde data toont aan de gebruiker.

### Entities (2 PUNTEN)
Maak de volgende klassen aan in de map `Entities`:
- `Store`
- `Product`

Gebruik het csv-bestand `stores_products.csv` als referentie voor de properties van deze klassen. Het is de bedoeling dat je de data uit het csv-bestand kan opslaan in objecten van deze klassen.
Zorg er ook voor dat een `Store`-object een lijst van `Product`-objecten kan bevatten.

### STREAMS (15PUNTEN)
Maak de klasse `FileService` aan en erf over van de interface `IFileService`.
Implementeer de methode `ReadFile` die het csv-bestand `stores_products.csv` inleest en de data teruggeeft als een lijst van `Store` objecten.

De data moet ingelezen worden op het moment dat er een object van de `StoreService`-klasse wordt aangemaakt (zie deel LINQ).

Zorg er ook voor dat de methode `ReadFile` de gebruiker op de hoogte brengt wanneer er iets fout loopt.

### LINQ (30 PUNTEN)
Implementeer alle methodes die in de interface `IStoreService` gedefinieerd zijn in de `StoreService` klasse. Voeg aan deze klasse ook een lijst van `Store`-objecten toe die automatisch gevuld wordt met de data die ingelezen wordt door de `FileService` klasse.

In de `IStoreService`-interface vind je de uitleg die bij elke methode hoort.
Werk elke methode uit. Gebruik hiervoor telkens de meest optimale LINQ query.

Test elke methode in de `Program.cs` file. Het niet testen van een methode kan leiden tot een lagere score.

### EVENTS (15 PUNTEN)
Maak een event aan dat alle acties van de FileService logt. Gebruik hiervoor de leerstof die je in de les gezien hebt.

Voor het loggen van acties, moet de volgende tekst getoond worden aan de gebruiker: `[DATETIME] - [NAAM VAN DE ACTIE] - [STATUS (GELUKT/GEFAALD)] - [FOUTMELDING INDIEN VAN TOEPASSING]`.
De methode die je gebruikt voor het loggen van acties (genaamd `Log`), moet in de LogService-klasse staan.

De volgende zaken moeten gelogd worden:
- Het aanmaken van de maplocatie.
- Loggen indien het bestand niet bestaat.
- Loggen indien het bestand succesvol ingelezen werd.
- Loggen indien het inlezen van het bestand mislukt.

# VEEL SUCCES!