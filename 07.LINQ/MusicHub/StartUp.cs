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
            string result = ExportAlbumsInfo(context, 9);
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
                    ReleaseDate = a.ReleaseDate.ToString(@"MM/dd/yyyy",CultureInfo.InvariantCulture),
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
            throw new NotImplementedException();
        }
    }
}
