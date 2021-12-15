namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            PlayInputDto[] playDtosInput = XmlConverter.Deserializer<PlayInputDto>(xmlString, "Plays");
            HashSet<Play> validPlaysModels = new HashSet<Play>();

            foreach (var playDto in playDtosInput.Distinct())
            {
                TimeSpan durationTime;
                if (!IsValid(playDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                bool durationIsValid = TimeSpan.TryParseExact(playDto.Duration, "c", CultureInfo.InvariantCulture, out durationTime);
                if (durationTime.Hours < 1 || !durationIsValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Play p = new Play
                {
                    Title = playDto.Title,
                    Duration = durationTime,
                    Rating = playDto.Rating,
                    Genre = Enum.Parse<Genre>(playDto.Genre),
                    Description = playDto.Description,
                    Screenwriter = playDto.Screenwriter
                };

                validPlaysModels.Add(p);
                sb.AppendLine(String.Format(SuccessfulImportPlay, playDto.Title, playDto.Genre, p.Rating));
            }

            context.Plays.AddRange(validPlaysModels);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            CastInputDto[] castDtoInput = XmlConverter.Deserializer<CastInputDto>(xmlString, "Casts");
            HashSet<Cast> validCast = new HashSet<Cast>();
            foreach (var castDto in castDtoInput)
            {
                if (!IsValid(castDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Cast c = new Cast()
                {
                    FullName = castDto.FullName,
                    IsMainCharacter = castDto.IsMainCharacter,
                    PhoneNumber = castDto.PhoneNumber
                };
                string isMain = c.IsMainCharacter ? "main" : "lesser";

                validCast.Add(c);
                sb.AppendLine(String.Format(SuccessfulImportActor, c.FullName, isMain));
            }
            context.Casts.AddRange(validCast);
            
            return sb.ToString().TrimEnd();

        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            throw new NotImplementedException();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
