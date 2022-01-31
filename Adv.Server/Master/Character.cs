using System.Numerics;
using Adv.Server.Util;
using Adv.Server.Util.Enums;

namespace Adv.Server.Master
{
    class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public byte Avatar { get; set; }
        public int ColorA { get; set; }
        public int ColorB { get; set; }
        public int ColorC { get; set; }
        public int ColorD { get; set; }
        public int Flags { get; set; }
        public bool IsAdmin { get; set; }
        public User User { get; set; }

        public int Health { get; set; }
        public Vector3 Position { get; set; }
        public Rotation Rotation { get; set; }
        public bool PvPEnabled { get; set; }


        public Character(string name, Location location, byte avatar, int colorA, int colorB, int colorC, int colorD, int flags, bool isAdmin, User user, int id = 0)
        {
            Name = name;
            Location = location;
            Avatar = avatar;
            ColorA = colorA;
            ColorB = colorB;
            ColorC = colorC;
            ColorD = colorD;
            Flags = flags;
            IsAdmin = isAdmin;
            User = user;
            Id = id;
            
            Health = 100;
            PvPEnabled = false;
        }
    }
}
