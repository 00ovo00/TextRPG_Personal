using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    enum GameMode
    {
        None,
        Lobby,  // 로비(시작화면)
        ShowState,  // 상태보기
        ShowInven,  // 인벤토리 보기
        GoStore,    // 상점
        BuyItem,    // 아이템 구입
        EquipItem,  // 장착관리
    }

    enum MessageType
    {
        Normal,   // 정상 입력 처리
        WrongInput, // 잘못된 입력 처리
        AlreadyBought,  // 중복 구매 방지
        PurchaseSucceed,    // 구매 완료
        LackGold,   // 재화 부족
    }

    public class Game
    {
        /* 게임 기본 세팅 */
        GameMode mode = GameMode.Lobby;
        MessageType msgtype = MessageType.Normal;

        Player player = new Player(1, "Newbie", PlayerType.Warrior, 10, 5, 100, 1500);
        Store store = new Store();
        Inventory inventory = new Inventory();
        Utility utility = new Utility();

        // 게임의 상태에 따라 적절한 처리를 진행하는 함수(사실상 메인의 역할)
        public void Process()
        {
            switch (mode)
            {
                case GameMode.Lobby:
                    ProcessLobby();
                    break;
                case GameMode.ShowState:
                    ProcessShowState();
                    break;
                case GameMode.ShowInven:
                    ProcessShowInven();
                    break;
                case GameMode.EquipItem:
                    ProcessEquipItem();
                    break;
                case GameMode.GoStore:
                    ProcessGoStore();
                    break;
                case GameMode.BuyItem:
                    ProcessBuyItem();
                    break;
            }
        }

        // 로비 화면을 처리하는 함수
        public void ProcessLobby()
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
            Console.WriteLine("1.상태 보기");
            Console.WriteLine("2.인벤토리");
            Console.WriteLine("3.상점");

            // 사용자 입력 처리
            int input = utility.InputFromUser(ref msgtype);
            switch (input)
            {
                case 1:
                    msgtype = MessageType.Normal;
                    mode = GameMode.ShowState;
                    break;
                case 2:
                    msgtype = MessageType.Normal;
                    mode = GameMode.ShowInven;
                    break;
                case 3:
                    msgtype = MessageType.Normal;
                    mode = GameMode.GoStore;
                    break;
                default:
                    msgtype = MessageType.WrongInput;
                    break;
            }
        }

        public void ProcessShowState()
        {
            mode = player.ProcessShowState(player, ref msgtype);
        }

        public void ProcessShowInven()
        {
            mode = inventory.ProcessShowInven(ref msgtype);
        }

        public void ProcessEquipItem()
        {
            mode = inventory.ProcessEquipItem(player, this, ref msgtype);
        }

        public void ProcessGoStore()
        {
            int gold = player.Gold;
            mode = store.ProcessGoStore(gold, this, ref msgtype);
        }

        public void ProcessBuyItem()
        {
            mode = store.ProcessBuyItem(player, ref msgtype, inventory);
        }
    }
}
