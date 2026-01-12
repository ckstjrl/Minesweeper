using MinesweeperProject.Models;
using MinesweeperProject.Services;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace MinesweeperProject.ViewModels
{
    public class RankingViewModel : ViewModelBase
    {
        private readonly MainViewModel _mainParent;
        private ObservableCollection<RankingEntry> _rankings;

        public ObservableCollection<RankingEntry> Rankings
        {
            get => _rankings;
            set => SetProperty(ref _rankings, value);
        }

        public ICommand ReturnToMenuCommand { get; }
        public ICommand ResetRankingsCommand { get; }

        public RankingViewModel(MainViewModel mainParent)
        {
            _mainParent = mainParent;
            Rankings = new ObservableCollection<RankingEntry>();

            ReturnToMenuCommand = new RelayCommand(o => ReturnToMenu());
            ResetRankingsCommand = new RelayCommand(o => ResetRankings());

            LoadRankings();
        }

        private void LoadRankings() // 랭킹 불러오기
        {
            string filePath = "rankings.json";
            if (!File.Exists(filePath)) return;

            try
            {
                string jsonString = File.ReadAllText(filePath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<RankingData>(jsonString, options);

                if (data != null && data.DifficultyRankings != null)
                {
                    var allEntries = data.DifficultyRankings.Values
                                        .SelectMany(list => list)
                                        .OrderBy(r => r.Time)
                                        .ToList();

                    for (int i = 0; i < allEntries.Count; i++)
                    {
                        allEntries[i].Rank = i + 1;
                    }

                    Rankings = new ObservableCollection<RankingEntry>(allEntries);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"랭킹 로드 오류: {ex.Message}");
            }
        }

        private void ResetRankings() // 랭킹 초기화 하기
        {
            var result = MessageBox.Show("모든 랭킹 기록을 초기화하시겠습니까?", "확인",
                                         MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                if (File.Exists("rankings.json")) File.Delete("rankings.json");
                Rankings.Clear();
            }
        }

        private void ReturnToMenu() //메인 메뉴로 돌아가기
        {
            _mainParent.ShowMainMenuView(_mainParent.Nickname ?? "Guest");
        }
    }
}