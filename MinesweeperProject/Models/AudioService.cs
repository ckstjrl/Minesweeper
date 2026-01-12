using System;
using System.IO;
using System.Media;
using System.Windows.Media;

namespace MinesweeperProject.Models
{
    public class AudioService
    {
        public static AudioService Instance { get; } = new AudioService();
        private MediaPlayer _bgmPlayer = new MediaPlayer();
        private MediaPlayer _sfxPlayer = new MediaPlayer();

        public void PlayBGM(string fileName)
        {
            // Resources/BGM 폴더 내의 파일을 실행 경로 기준으로 찾음
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "BGM", fileName);
            if (File.Exists(path))
            {
                _bgmPlayer.Open(new Uri(path));
                _bgmPlayer.MediaEnded += (s, e) => { _bgmPlayer.Position = TimeSpan.Zero; _bgmPlayer.Play(); }; // 반복 재생
                _bgmPlayer.Play();
            }
        }

        public void SetBgmVolume(double volume) => _bgmPlayer.Volume = volume;
        public void SetSfxVolume(double volume) => _sfxPlayer.Volume = volume;

        public void SetBgmMute(bool isMuted) => _bgmPlayer.IsMuted = isMuted;
        public void SetSfxMute(bool isMuted) => _sfxPlayer.IsMuted = isMuted;

        public void PlaySFX(string fileName)
        {
            // 1. 파일 확장자가 .wav인지 확인 (SoundPlayer는 mp3 지원 안함)
            string wavFileName = Path.ChangeExtension(fileName, ".wav");
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "SFX", wavFileName);

            if (File.Exists(path))
            {
                try
                {
                    using (SoundPlayer player = new SoundPlayer(path))
                    {
                        player.Play();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"SFX 재생 오류: {ex.Message}");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"파일 없음: {path}");
            }
        }
    }

    // 노래 정보를 담는 간단한 클래스
    public class MusicItem
    {
        public string Title { get; set; }
        public string FileName { get; set; }
        public MusicItem(string title, string fileName) { Title = title; FileName = fileName; }
    }
}