using NuGet;
using System;
using System.IO;
using System.Text;

namespace UiPath.PowerShell.Tests.Util
{
    public class TestRandom
    {
        public static class CharSet
        {
            public const string Alpha = "abcdefghijklmnopqrstuvwzx";
            public const string Numeric = "0123456789";
            public const string Special = "`~!@#$%^&*()-_=+[{]}|;:/?.>,<";  // \ ' and " are intentionally removed to avoid URL escaping issues that autorest client cannot handle correctly
        }

        [Flags]
        public enum CharType
        {
            AlphaUpper  = 1,
            AlphaLower  = 2,
            Numeric     = 4,
            Special     = 8
        }

        public Random Random { get; set; }

        public TestRandom()
        {
            Random = new Random();
        }

        internal string RandomPassword(int minLength = 10, int maxLength = 15)
        {
            var length = Random.Next(minLength, maxLength);

            return RandomAlphaNumeric(length, minLength / 2);
        }


        internal string RandomEmail()
        {
            return $"{RandomAlphaNumeric()}@example.com";
        }

        public string RandomAlphaNumeric(int minLength = 10, int maxLength = 15)
        {
            return RandomString(minLength, maxLength, CharType.AlphaUpper | CharType.AlphaLower | CharType.Numeric);
        }

        public string RandomString()
        {
            return RandomString(10, 15);
        }

        public string RandomText(int minCount = 100, int maxCount = 1000)
        {
            var sb = new StringBuilder();
            int count = Random.Next(minCount, maxCount);
            string delimiter = "";
            for(int i=0; i<count;++i)
            {
                sb.Append(delimiter);
                sb.Append(RandomAlphaNumeric(1));

                // Don't gen newlines for less than 10 words
                var roll = Random.Next(0, maxCount < 10 ? 95 : 100);
                if (roll < 70)
                {
                    delimiter = " ";
                }
                else if (roll < 95)
                {
                    delimiter = ".";
                }
                else
                {
                    delimiter = Environment.NewLine;
                }
            }
            return sb.ToString();
        }

        public PackageSpec RandomPakcageSpec()
        {
            return new PackageSpec
            {
                Id = RandomAlphaNumeric(),
                Title = RandomText(1,5),
                Authors = RandomEmail(),
                Version = new Version(Random.Next(1, 10), Random.Next(0,100), Random.Next(0,1000)),
                Description = RandomText(5, 9),
                ReleaseNotes = RandomText(15, 25)
            };
        }

        public TestFileFixture RandomPackage(PackageSpec spec)
        {
            string fileName = Path.Combine(Path.GetTempPath(), spec.Id + "." + spec.Version + ".nupkg");

            Manifest manifest = new Manifest();
            manifest.Metadata = new ManifestMetadata
            {
                Id = spec.Id,
                Authors = spec.Authors,
                Description = spec.Description,
                Title = spec.Title,
                Version = spec.Version.ToString(),
                ReleaseNotes  = spec.ReleaseNotes,
            };

            using (var contentFile = new TestFileFixture { FileName = Path.GetTempFileName() })
            {

                using (var fs = File.Create(fileName))
                {
                    var pb = new PackageBuilder
                    {
                        Id = spec.Id,
                    };

                    pb.Populate(manifest.Metadata);

                    pb.Files.Add(new PhysicalPackageFile()
                    {
                        SourcePath = contentFile.FileName,
                        TargetPath = "content"
                    });

                    pb.Save(fs);
                }
            }

            return new TestFileFixture { FileName = fileName };
        }

        public string RandomString(int minLength, int maxLength, CharType charType = CharType.AlphaUpper | CharType.AlphaLower | CharType.Numeric | CharType.Special)
        {
            var length = Random.Next(minLength, maxLength);
            StringBuilder sb = new StringBuilder(length);

            var types = (charType.HasFlag(CharType.AlphaUpper) ? 1 : 0) +
                (charType.HasFlag(CharType.AlphaLower) ? 1 : 0) +
                (charType.HasFlag(CharType.Numeric) ? 1 : 0) +
                (charType.HasFlag(CharType.Special) ? 1 : 0);

            var probabilityAlphaUpper = charType.HasFlag(CharType.AlphaUpper) ? Math.Max(length - types, 1) : 0;
            var probabilityAlphaLower = charType.HasFlag(CharType.AlphaLower) ? Math.Max(length - types, 1) : 0;
            var probabilityNumeric = charType.HasFlag(CharType.Numeric) ? Math.Max(length - types, 1) : 0;
            var probabilitySpecial = charType.HasFlag(CharType.Special) ? Math.Max(length - types, 1) : 0;

            while (length > 0)
            {
                Char c = default(Char);
                var roll = Random.Next(probabilityAlphaUpper + probabilityAlphaLower + probabilityNumeric + probabilitySpecial);
                if (roll > probabilityAlphaUpper + probabilityAlphaLower + probabilityNumeric)
                {
                    c = CharSet.Special[Random.Next(CharSet.Special.Length)];
                    --probabilitySpecial;
                }
                else if (roll > probabilityAlphaUpper + probabilityAlphaLower )
                {
                    c = CharSet.Numeric[Random.Next(CharSet.Numeric.Length)];
                    --probabilityNumeric;
                }
                else if (roll > probabilityAlphaUpper )
                {
                    c = Char.ToUpper(CharSet.Alpha[Random.Next(CharSet.Alpha.Length)]);
                    --probabilityAlphaUpper;
                }
                else
                {
                    c = Char.ToLower(CharSet.Alpha[Random.Next(CharSet.Alpha.Length)]);
                    --probabilityAlphaLower;
                }
                sb.Append(c);
                --length;
            }

            return sb.ToString();
        }
    }
}
