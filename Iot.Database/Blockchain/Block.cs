using System.Security.Cryptography;
using System.Text;

namespace Iot.Database.Blockchain;

public class Block
{
    public Guid Id { get; set; }
    public BsonValue Data { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string PreviousHash { get; set; }
    public string Hash { get; set; }

    public Block() { }

    public Block(string previousHash, BsonValue data)
    {
        Timestamp = DateTime.UtcNow;
        PreviousHash = previousHash;
        Data = data;
        Hash = CalculateHash();
    }

    public string CalculateHash()
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            string rawData = $"{Timestamp}-{PreviousHash ?? ""}-{Data}";
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
