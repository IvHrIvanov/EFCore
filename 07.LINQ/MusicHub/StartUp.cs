namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);
            string result = ExportSongsAboveDuration(context, 4);
            Console.WriteLine(result);
            //Test your solutions here
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder sb = new StringBuilder();
            var albumInfo = context.Albums
                .ToArray()
                .Where(x => x.Producer.Id == producerId)
                .OrderByDescending(p => p.Price)
                .Select(a => new
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString(@"MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = a.Producer.Name,
                    AlbumSongs = a.Songs
                    .ToArray()
                    .Select(s => new
                    {
                        SongName = s.Name,
                        Price = s.Price.ToString("0.00"),
                        SongWriter = s.Writer.Name
                    })
                    .OrderByDescending(s => s.SongName)
                    .ThenBy(w => w.SongWriter),
                    TotalPrice = a.Price.ToString("0.00")
                });

            foreach (var ai in albumInfo)
            {
                int count = 1;
                sb.AppendLine($"-AlbumName: {ai.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {ai.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {ai.ProducerName}");
                sb.AppendLine($"-Songs:");

                foreach (var song in ai.AlbumSongs)
                {
                    sb.AppendLine($"---#{count}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.Price}");
                    sb.AppendLine($"---Writer: {song.SongWriter}");

                    count++;
                }
                sb.AppendLine($"-AlbumPrice: {ai.TotalPrice}");

            }

            return sb.ToString().TrimEnd();

        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            StringBuilder sb = new StringBuilder();
            var songsInfo = context.Songs
                .ToArray()
                .Where(d => d.Duration.Minutes > duration)
                .Select(s => new
                {
                    SongName = s.Name,
                    Writer = s.Writer.Name,

                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration.ToString("c", CultureInfo.CurrentCulture),
                    Performer = s.SongPerformers
                    .ToArray()
                    .Select(x => new
                    {
                        
                        FullName = String.Join(" ",x.Performer.FirstName,x.Performer.LastName)
                    })

                })
                .OrderBy(s => s.SongName)
                .ThenBy(w => w.Writer)
                .ThenBy(p => p.Performer);
            int count = 1;
            foreach (var s in songsInfo)
            {
                sb.AppendLine($"-Song #{count}");
                sb.AppendLine($"---SongName: {s.SongName}");
                sb.AppendLine($"---Writer: {s.Writer}");
                foreach (var p in s.Performer)
                {
                    sb.AppendLine($"---Performer: {p.FullName}");
                }
                sb.AppendLine($"---AlbumProducer: {s.AlbumProducer}");
                sb.AppendLine($"---Duration: {s.Duration}");
                count++;
            }
            return sb.ToString().TrimEnd();
        }
    }
}
