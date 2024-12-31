using System;
using System.Collections.Generic;
using NAudio.Wave;

public class MyProgram
{
    // Daftar untuk menyimpan riwayat lagu yang telah diputar oleh pengguna
    static List<string> history = new List<string>();

    public static void Main(string[] args)
    {
        string username = ""; // Input User
        string password = ""; // Input Password
        login(ref username, ref password); // Memanggil metode login untuk mendapatkan username dan password

        // Kata sandi dan username
        string validUsername = "Admin";
        string validPassword = "Admin";

        if (username == validUsername && password == validPassword)
        {
            Console.WriteLine("Login Berhasil");
        }
        else
        {
            Console.WriteLine("Username atau Password Salah");
            return; // Menghentikan eksekusi jika login gagal
        }

        // Daftar lagu dalam array dengan genre
        Dictionary<string, List<string>> songsByGenre = new Dictionary<string, List<string>>
        {
            { "Random Song", new List<string> { "lagu ganteng.mp3", "MELOWWW.mp3" } },
            { "R&G Song", new List<string> { "Party Anthem.mp3", "I Wanna Be Yours.mp3", "505.mp3" } },
            { "Pop Song", new List<string> { "MawarJingga.mp3", "TerlaluTinggi.mp3", "Lantas.mp3" } }
        };

        try
        {
            int pilihan = 0;
            while (pilihan != 7) // Loop utama untuk menampilkan menu yang akan dipilih oleh user
            {
                Console.WriteLine("===PILIH KATEGORI LAGU===");
                Console.WriteLine("==========================");
                Console.WriteLine("1. Cari Lagu");
                Console.WriteLine("2. Lihat dan Hapus History");
                Console.WriteLine("3. Filter Lagu Berdasarkan Genre");
                Console.WriteLine("4. Exit");
                Console.Write("Masukkan Pilihan: ");
                pilihan = Convert.ToInt32(Console.ReadLine()); // Opsi Membaca Pilihan user

                string filePath = ""; // Variabel untuk menyimpan jalur path (file) lagu yang akan dipilih

                switch (pilihan)
                {
                    case 1:
                        SearchAndPlaySong(songsByGenre);
                        break;

                    case 2:
                        ManageHistory();
                        break;

                    case 3:
                        FilterSongsByGenre(songsByGenre);
                        break;

                    case 4:
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
                using (var audioFile = new AudioFileReader(filePath)) // Membuat pembaca file audio dan perangkat output
                using (var outputDevice = new WaveOutEvent()) // memulai pemutaran audio mp3
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                    Console.WriteLine("Menikmati musik... Tekan 'Spasi' untuk berhenti.");

                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Spacebar) // key untuk stop lagu (bisa diganti)
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

    static void ManageHistory() // Metode untuk mengelola riwayat lagu yang diputar oleh user
    {
        if (history.Count == 0) // Memeriksa apakah riwayat lagu kosong
        {
            Console.WriteLine("History kosong.");
            return;
        }

        Console.WriteLine("===HISTORY PEMUTARAN LAGU===");
        for (int i = 0; i < history.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {history[i]}"); // Menampilkan setiap lagu dalam riwayat
        }

        Console.WriteLine("Apakah Anda ingin menghapus history? (y/n)"); // input hapus history
        string choice = Console.ReadLine();
        if (choice?.ToLower() == "y")
        {
            history.Clear();
            Console.WriteLine("History telah dihapus.");
        }
    }

    // Metode untuk mencari dan memutar lagu berdasarkan kata kunci
    static void SearchAndPlaySong(Dictionary<string, List<string>> songsByGenre)
    {
        Console.WriteLine("Masukkan kata kunci pencarian: ");
        string keyword = Console.ReadLine()?.ToLower();

        List<string> results = new List<string>();

        // Mencari di setiap kategori lagu
        foreach (var genre in songsByGenre.Keys)
        {
            results.AddRange(songsByGenre[genre].FindAll(song => song.ToLower().Contains(keyword)));
        }

        if (results.Count == 0)
        {
            Console.WriteLine("Tidak ada lagu yang cocok dengan kata kunci.");
        }
        else
        {
            Console.WriteLine("Hasil Pencarian:");
            for (int i = 0; i < results.Count; i++)
            {
                Console.WriteLine($"{i}. {results[i]}"); // Menampilkan nomor dan nama lagu
            }

            Console.Write("Pilih nomor lagu untuk diputar (0 untuk batal): ");
            int pilihan = Convert.ToInt32(Console.ReadLine());

            if (pilihan > 0 && pilihan <= results.Count)
            {
                string selectedSong = results[pilihan - 1]; // Mengambil lagu yang dipilih dari hasil pencarian
                history.Add(selectedSong); // Menambah lagu yang dipilih ke riwayat
                PlayMusic(selectedSong); // Memutar lagu yang dipilih
            }
            else
            {
                Console.WriteLine("Tidak ada lagu yang dipilih.");
            }
        }
    }

    // Metode untuk memfilter dan menampilkan lagu berdasarkan genre
    static void FilterSongsByGenre(Dictionary<string, List<string>> songsByGenre)
    {
        Console.WriteLine("===FILTER LAGU BERDASARKAN GENRE===");
        Console.WriteLine("Pilih genre : ");
        int index = 1;
        foreach (var genre in songsByGenre.Keys)
        {
            Console.WriteLine($"{index++}. {genre}");
        }

        Console.Write("Masukkan nomor genre: ");
        int genreChoice = Convert.ToInt32(Console.ReadLine());

        if (genreChoice > 0 && genreChoice <= songsByGenre.Count)
        {
            string selectedGenre = new List<string>(songsByGenre.Keys)[genreChoice - 1];
            Console.WriteLine($"===DAFTAR LAGU {selectedGenre.ToUpper()}===");
            foreach (var song in songsByGenre[selectedGenre])
            {
                Console.WriteLine(song); // Menampilkan seluruh lagu berdasarkan genre yang dipilih
                Console.WriteLine("Pilih nomor lagu untuk diputar (0 untuk batal): ");
            }
            int pilihan = Convert.ToInt32(Console.ReadLine());

            if (pilihan > 0 && pilihan <= songsByGenre.Count)
            {
                string selectedSong = songsByGenre[selectedGenre][pilihan - 1]; // Mengambil lagu yang dipilih dari hasil pencarian
                history.Add(selectedSong); // Menambah lagu yang dipilih ke riwayat
                PlayMusic(selectedSong); // Memutar lagu yang dipilih
            }
            else
            {
                Console.WriteLine("Tidak ada lagu yang dipilih.");
            }
        }
        else
        {
            Console.WriteLine("Pilihan genre tidak valid.");
        }
    }
}