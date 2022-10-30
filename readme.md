### IronM Backend Api
# Datamodel
Speichert Fragen, Teamname und Antworten. 
Beim Anmelden gibt der Server den Clients einen Bearer Token fuer die Zukuenftige kommunikation.
Die Daten werden in einer InMemory SQL Datenbank gespeichert. 

# RestApi
RestApi zum registrieren von Lobbies und zum anmelden von clients 
IronApp/Controllers/QuizRegistrationController.cs
RestApi fuer alle Quiz internen interaktionen 
IronApp/Controllers/QuizController
Alle Dto-Klassen zur sicheren Kommunikation in 
IronApp/Model/ExchangeModels
Alle Datenbankmodels in 
IronApp/Model/QuizEntityModels

# SignalR
SignalR, websocket implementierung, zum live updaten von Clients. Broadcastet neue Antworten damit die Jury neue Antworten live sofort sehen kann.

# Entity Framework
ORM zum einfachen erstellung des Datenbankmodels. 

