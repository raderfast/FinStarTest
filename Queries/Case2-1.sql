SELECT "ClientName", COUNT("ClientContacts"."ContactValue")
FROM public."Clients" 
LEFT JOIN "ClientContacts" on "ClientId" = "Clients"."Id"
GROUP BY "Clients"."Id" 
ORDER BY "Clients"."Id" ASC 