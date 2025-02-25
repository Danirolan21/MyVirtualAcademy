using System.Security.Cryptography;
using System.Text;

namespace MyVirtualAcademy.Helper
{
    public class HelperCryptography
    {
        public static string GenerateSalt()
        {
            Random random = new Random();
            string salt = "";
            for (int i = 1; i <= 50; i++)
            {
                int aleat = random.Next(1, 255);
                char letra = Convert.ToChar(aleat);
                salt += letra;
            }
            return salt;
        }

        public static bool CompararArrays(byte[] a, byte[] b)
        {
            bool iguales = true;
            if (a.Length != b.Length)
            {
                iguales = false;
            }
            else
            {
                for (int i = 1; i < a.Length; i++)
                {
                    if (a[i].Equals(b[i]) == false)
                    {
                        iguales = false;
                        break;
                    }
                }
            }
            return iguales;
        }


        public static byte[] EncryptPassword(string password, string salt)
        {
            string contenido = password + salt;
            SHA256 managed = SHA256.Create();
            byte[] salida = Encoding.UTF8.GetBytes(contenido);
            //CREAMOS EL BUCLE DEL CIFRADO CON ITERACIONES
            for (int i = 1; i <= 20; i++)
            {
                salida = managed.ComputeHash(salida);
            }
            managed.Clear();
            return salida;
        }
    }
}
