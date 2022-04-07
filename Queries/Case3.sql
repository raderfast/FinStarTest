SELECT d."Id", Min(d."Dt") AS Sd, Min(d2."Dt") AS Ed
FROM "Dates" d
LEFT OUTER JOIN "Dates" d2 ON (d2."Dt" > d."Dt" AND d."Id" = d2."Id") 
WHERE d2."Dt" IS NOT NULL
GROUP BY d."Id", d."Dt" 
ORDER BY d."Id", d."Dt" ASC