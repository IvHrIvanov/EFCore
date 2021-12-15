namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using System.Linq;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            TheatreDto[] theatres = context.Theatres
                .Where(noh => noh.Tickets.Count >= 20 && noh.NumberOfHalls >= numbersOfHalls )
                .ToArray()
                .Select(td => new TheatreDto
                {
                    Name = td.Name,
                    Halls = td.NumberOfHalls,
                    TotalIncome = td.Tickets
                      .Where(rn =>  rn.RowNumber <= 5 && rn.RowNumber >= 1).ToArray().Sum(pr => pr.Price),
                    Tickets = td.Tickets
                      .Where(rn => rn.RowNumber >= 1 && rn.RowNumber <= 5).ToArray()
                    .Select(x => new TicketDto()
                    {
                        RowNumber = x.RowNumber,
                        Price = x.Price

                    })
                       .OrderByDescending(x => x.Price)
                       .ToArray()
                })
                .OrderByDescending(t => t.Halls)
                .ThenBy(t => t.Name)
                .ToArray();

            string result = JsonConvert.SerializeObject(theatres, Formatting.Indented);
            return result;
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            var plays = context.Plays
                .Where(r => r.Rating <= rating).ToList()
                .Select(p => new PlayDto
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c", CultureInfo.InvariantCulture),
                    Genre = p.Genre.ToString(),
                    Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(),      
                    Actors = p.Casts
                     .Where(imc => imc.IsMainCharacter)
                     .Select(act => new ActorDto()
                     {
                         FullName = act.FullName,
                         MainCharacter = $"Plays main character in '{p.Title}'."
                     })
                        .OrderByDescending(fn => fn.FullName)
                        .ToList()
                })
                .OrderBy(x => x.Title)
                .ThenByDescending(x => x.Genre)
                .ToList();
            var resulXml = XmlConverter.Serialize(plays, "Plays");
            return resulXml;

        }
    }
}
