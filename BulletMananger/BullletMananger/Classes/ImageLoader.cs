using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PicAnimator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BulletManager
{
    public static class ImageLoader
    {
        public struct CharacterData
        {
            /// <summary>
            /// The list of character animations
            /// </summary>
            public Dictionary<string, Mover> CharacterAnims;
            /// <summary>
            /// The list of character attcks
            /// </summary>
            public List<Attack> CharacterAttacks;
            /// <summary>
            /// The list of bullets owned by the character
            /// </summary>
            public List<Bullet> Bullets;
            /// <summary>
            /// The list of TimerBullets for an attack. DO NOT USE OUTSIDE OF ImageLoader
            /// </summary>
            internal List<TimerBullet> AttackBullets;
            /// <summary>
            /// Holds all of the loaded character data loaded from their file
            /// </summary>
            /// <param name="anims"></param>
            /// <param name="bullets"></param>
            /// <param name="attacks"></param>
            /// <param name="attackBullets"></param>
            public CharacterData(
                Dictionary<string, Mover> anims, List<Bullet> bullets,
                List<Attack> attacks, List<TimerBullet> attackBullets)
            {
                CharacterAnims = anims;
                CharacterAttacks = attacks;
                AttackBullets = attackBullets;
                Bullets = bullets;
            }
        }

        static bool isLoaded;

        private static Dictionary<string, Mover> BulletEffects;
        static Dictionary<Type, CharacterData> LoadedCharacters;
        static Dictionary<Type, Bullet> LoadedBullets;

        private const string CHARACTER_PATH = @"Assets\Characters\";
        private const string BULLET_PATH = @"Assets\Bullets\";

        #region File Keywords
        public const string ATTACK1 = "ATTACK1";
        public const string ATTACK2 = "ATTACK2";
        public const string ATTACK3 = "ATTACK3";
        public const string DYING = "DYING";
        public const string ENTER = "ENTER";
        public const string IDLE = "IDLE";
        public const string HIT = "HIT";
        public const string SHOCK1 = "SHOCK1";
        public const string SHOCK2 = "SHOCK2";
        public const string SHOCK3 = "SHOCK3";
        public const string WIN = "WIN";
        public const string LOSE = "LOST";
        public const string BULLET = "BULLET";
        public const string ATTACK_FORM = "ATTACK_FORM";
        public const string END_ATTACK_FORM = "END_ATTACK_FORM";
        public const string FILE_NAME = "FILE_NAME";
        public const string TYPE = "TYPE";
        public const char COMMENT = '*';
        #endregion
        #region Data Indices
        public const int TAG = 0;
        public const int X_POS = 1;
        public const int Y_POS = 2;
        public const int WIDTH = 3;
        public const int HEIGHT = 4;
        public const int EXTRA_FRAMES = 5;
        public const int FPS = 6;
        public const int IS_HORIZONTAL = 7;
        public const int ATTACK_NAME = 1;
        public const int CAN_MOVE = 2;


        public const int BULLET_NAME = 1;
        public const int BULLET_FILENAME = 2;

        public const int BULLET_INDEX = 0;
        public const int BULLET_X = 1;
        public const int BULLET_Y = 2;
        public const int BULLET_SPAWN_TIME = 3;
        public const int BULLET_TYPE = 4;
        #endregion

        public static CharacterData LoadPlayerAssets(Game game, Type name)
        {
            if (!isLoaded)
            {
                SetupDictionaries(game);
            }
            if (!LoadedCharacters.Keys.Contains(name))
            {
                StreamReader sr = new StreamReader(CHARACTER_PATH + name.ToString());
                Texture2D texture = new Texture2D(game.GraphicsDevice, 0, 0);
                CharacterData results = new CharacterData(
                    new Dictionary<string, Mover>(), new List<Bullet>(),
                    new List<Attack>(), new List<TimerBullet>());
                string[] data;

                while (!sr.EndOfStream)
                {
                    data = sr.ReadLine().Split(',');
                    switch (data[TAG])
                    {
                        case FILE_NAME:
                            texture = Texture2D.FromStream(game.GraphicsDevice, new FileStream(sr.ReadLine(), FileMode.Open));
                            break;
                        case ATTACK1:
                        case ATTACK2:
                        case ATTACK3:
                        case ENTER:
                        case IDLE:
                        case HIT:
                        case SHOCK1:
                        case SHOCK2:
                        case SHOCK3:
                        case WIN:
                        case LOSE:
                            if (data[1] != "")
                            {
                                results.CharacterAnims.Add(data[TAG], MakeMover(texture, data));
                            }
                            break;
                        case BULLET:
                            StreamReader bReader = new StreamReader(BULLET_PATH + data[BULLET_FILENAME]);
                            string[] bdata = new string[2];
                            //In Bullet File
                            texture = Texture2D.FromStream(game.GraphicsDevice, new FileStream(bReader.ReadLine(), FileMode.Open));
                            Dictionary<string, Mover> movers = new Dictionary<string, Mover>();
                            Bullet.ElementTypes bullet_type = Bullet.ElementTypes.Fire;
                            while (!bReader.EndOfStream)
                            {
                                bdata = bReader.ReadLine().Split(',');
                                switch (bdata[TAG])
                                {
                                    case IDLE:
                                    case DYING:
                                        movers.Add(data[TAG], MakeMover(texture, data));
                                        break;
                                    case TYPE:
                                        try
                                        {
                                            bullet_type = (Bullet.ElementTypes)ToNumber(bdata[BULLET_TYPE]);
                                        }
                                        catch (Exception e)
                                        {
                                            bullet_type = Bullet.ElementTypes.Metal;
                                        }
                                        break;
                                }
                            }
                            bReader.Close();
                            results.Bullets.Add(new Bullet(Vector2.Zero, true, movers, bullet_type, data[BULLET_NAME]));
                            break;
                        case ATTACK_FORM:
                            string attackName = data[ATTACK_NAME];
                            bool canMove = data[CAN_MOVE].ToUpper() == "TRUE" ? true : false;
                            data = sr.ReadLine().Split(',');
                            do
                            {
                                int spawnTime = ToNumber(data[BULLET_SPAWN_TIME]);
                                int index = ToNumber(data[BULLET_INDEX]);
                                results.AttackBullets.Add(new TimerBullet(spawnTime, results.Bullets[index],
                                    new Vector2(ToNumber(data[BULLET_X]), ToNumber(data[BULLET_Y]))));
                                data = sr.ReadLine().Split(',');
                            } while (data[0] != END_ATTACK_FORM);
                            results.CharacterAttacks.Add(new Attack(null, attackName, canMove, results.AttackBullets));
                            data = sr.ReadLine().Split(',');
                            break;
                        default:
                            if (data[TAG][0] == COMMENT)
                                continue;
                            break;
                    }
                }
                LoadedCharacters.Add(name, results);
                return results;
            }
            else return LoadedCharacters[name];
        }

        private static Mover MakeMover(Texture2D texture, string[] data)
        {
            int width = ToNumber(data[WIDTH]);
            int height = ToNumber(data[HEIGHT]);
            Mover result = new Mover(data[TAG],
                new Frame(texture, new Rectangle(ToNumber(data[X_POS]), ToNumber(data[Y_POS]),
                width, height),
                ToNumber(data[FPS])),
                -1, 0, 0, ToNumber(data[EXTRA_FRAMES]),
                data[IS_HORIZONTAL].ToUpper() == "TRUE"/*true*/ ? width : 0,
                data[IS_HORIZONTAL].ToUpper() == "TRUE"/*true*/ ? 0 : height);
            return result;
        }

        private static int ToNumber(string input)
        {
            string data = input.Split('.')[0];

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (data[i] == j.ToString()[0])
                    {
                        break;
                    }
                    else if (data[i] != j.ToString()[0] && j == 9)
                        return 0;
                }
            }

            return int.Parse(data);
        }

        public static void LoadBulletEffectsAssets(Game game)
        {

        }

        private static void SetupDictionaries(Game game)
        {
            LoadedCharacters = new Dictionary<Type, CharacterData>();
            BulletEffects = new Dictionary<string, Mover>();
            LoadedBullets = new Dictionary<Type, Bullet>();
        }
    }
}
