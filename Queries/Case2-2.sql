SELECT "Clients"."ClientName" 
FROM public."Clients" 
LEFT JOIN "ClientContacts" on "ClientId" = "Clients"."Id"
GROUP BY "Clients"."Id" 
HAVING COUNT("ClientContacts"."ClientId") > 2Dates 
ORDER BY "Clients"."Id" ASC 