using System;
using System.Collections.Generic;
using NAudio.Wave;

public class MyProgram
{
    static List<string> history = new List<string>();

    public static void Main(string[] args)
    {
        string username = "";
        string password = "";
        login(ref username, ref password);

        string validUsername = "admin";
        string validPassword = "admin1234";

        if (username == validUsername && password == validPassword)
        {
            Console.WriteLine("Login Berhasil");
        }
        else
        {
            Console.WriteLine("Username atau Password Salah");
            return; // Menghentikan eksekusi jika login gagal
        }

        // Daftar lagu dalam array
        string[] popSongs = { "lagu ganteng.mp3", "MELOWWW.mp3" };
        string[] articMonkeySongs = { "Party Anthem.mp3", "I Wanna Be Yours.mp3", "505.mp3" };
        string[] juicyLuicySongs = { "MawarJingga.mp3", "TerlaluTinggi.mp3", "Lantas.mp3" };

        try
        {
            int pilihan = 0;
            while (pilihan != 5)
            {
                Console.WriteLine("===PILIH KATEGORI LAGU===");
                Console.WriteLine("==========================");
                Console.WriteLine("1. Pop");
                Console.WriteLine("2. Artic Monkey");
                Console.WriteLine("3. Juicy Luicy");
                Console.WriteLine("4. Lihat dan Hapus History");
                Console.WriteLine("5. Cari Lagu");
                Console.WriteLine("6. Exit");
                Console.Write("Masukkan Pilihan: ");
                pilihan = Convert.ToInt32(Console.ReadLine());

                string filePath = "";

                switch (pilihan)
                {
                    case 1:
                        Console.WriteLine("===DAFTAR LAGU POP===");
                        for (int i = 0; i < popSongs.Length; i++)
                        {
                            Console.WriteLine($"{i + 1}. {popSongs[i]}");
                        }
                        Console.Write("Masukkan Pilihan Lagu: ");
                        int popPilihan = Convert.ToInt32(Console.ReadLine());
                        if (popPilihan > 0 && popPilihan <= popSongs.Length)
                        {
                            filePath = popSongs[popPilihan - 1];
                        }
                        break;

                    case 2:
                        Console.WriteLine("===DAFTAR LAGU ARTIC MONKEY===");
                        for (int i = 0; i < articMonkeySongs.Length; i++)
                        {
                            Console.WriteLine($"{i + 1}. {articMonkeySongs[i]}");
                        }
                        Console.Write("Masukkan Pilihan Lagu: ");
                        int amPilihan = Convert.ToInt32(Console.ReadLine());
                        if (amPilihan > 0 && amPilihan <= articMonkeySongs.Length)
                        {
                            filePath = articMonkeySongs[amPilihan - 1];
                        }
                        break;

                    case 3:
                        Console.WriteLine("===DAFTAR LAGU JUICY LUICY===");
                        for (int i = 0; i < juicyLuicySongs.Length; i++)
                        {
                            Console.WriteLine($"{i + 1}. {juicyLuicySongs[i]}");
                        }
                        Console.Write("Masukkan Pilihan Lagu: ");
                        int juicyPilihan = Convert.ToInt32(Console.ReadLine());
                        if (juicyPilihan > 0 && juicyPilihan <= juicyLuicySongs.Length)
                        {
                            filePath = juicyLuicySongs[juicyPilihan - 1];
                        }
                        break;

                    case 4:
                        ManageHistory();
                        break;

                    case 5:
                        Console.WriteLine("Keluar dari program...");
                        return;

                    default:
                        Console.WriteLine("Pilihan Tidak ada.");
                        continue;
                }

                // Setelah kategori dan lagu dipilih, kita akan memutar lagu
                if (!string.IsNullOrEmpty(filePath))
                {
                    history.Add(filePath);
                    PlayMusic(filePath);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Terjadi kesalahan: " + ex.Message);
        }
        finally
        {
            Console.WriteLine("Terima Kasih telah menggunakan program kami");
        }
    }

    static void login(ref string username, ref string password)
    {
        Console.WriteLine("Masukkan Nama Pengguna:");
        username = Console.ReadLine();

        Console.WriteLine("Masukkan Password:");
        password = Console.ReadLine();
    }

    

    // Metode untuk memutar musik
    static void PlayMusic(string filePath)
    {
        while (true)
        {
            try
            {
                using (var audioFile = new AudioFileReader(filePath))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                    Console.WriteLine("Menikmati musik... Tekan 'Spasi' untuk berhenti.");

                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Spacebar)
                        {
                            Console.WriteLine("Pemutaran dihentikan.");
                            outputDevice.Stop();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Terjadi kesalahan dalam memutar musik: " + ex.Message);
            }
        }
    }

    static void ManageHistory()
    {

        if (history.Count == 0)
        {
            Console.WriteLine("History kosong.");
            return;
        }

        Console.WriteLine("===HISTORY PEMUTARAN LAGU===");
        for (int i = 0; i < history.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {history[i]}");
        }
 
        Console.WriteLine("Apakah Anda ingin menghapus history? (y/n)");
        string choice = Console.ReadLine();
        if (choice?.ToLower() == "y")
        {
            history.Clear();
            Console.WriteLine("History telah dihapus.");
        }
     
    }
}