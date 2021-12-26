namespace MusicHub
{
    using System;
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
                .Where(x => x.Producer.Id == producerId)
                .Select(a => new
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy"),
                    ProducerName = a.Producer.Name,
                    AlbumSong = a.Songs.Select(s => new
                    {
                        SongName = s.Name,
                        Price = s.Price.ToString("0.00"),
                        SongWriter = s.Writer.Name
                    })
                });
            decimal albumPrice = 0;
            foreach (var ai in albumInfo)
            {
                int count = 1;
                sb.AppendLine($"-AlbumName: {ai.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {ai.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {ai.ProducerName}");
                foreach (var song in ai.AlbumSong)
                {              
                    sb.AppendLine($"---#{count}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.Price:f2}");
                    sb.AppendLine($"---Writer: {song.SongWriter}");

                    albumPrice += decimal.Parse(song.Price);
                    count++;
                }
                sb.AppendLine($"-AlbumPrice: {albumPrice}");

            }
            return sb.ToString().TrimEnd();

        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            throw new NotImplementedException();
        }
    }
}
