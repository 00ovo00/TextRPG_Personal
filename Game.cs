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
        GoStore,    // 상점가기
        BuyItem,    // 아이템 구입
        SellItem,    // 아이템 판매
        EquipItem,  // 장착관리
        GoShelter,  // 휴식하기
    }

    enum MessageType
    {
        Normal,   // 정상 입력 처리
        WrongInput, // 잘못된 입력 처리
        AlreadyBought,  // 중복 구매 방지
        PurchaseSucceed,    // 구매 완료
        SaleSucceed,    // 판매 완료
        LackGold,   // 재화 부족
        RestSucceed, // 휴식 완료
        RestFailed, // 휴식 실패
    }
    enum PrintType
    {
        StoreMain,  // 상점 메인 화면
        StoreBuy, // 상점 구입 화면
        StoreSell, // 상점 판매 화면
        InvenMain, // 인벤토리 메인 화면
        InvenEquip, // 인벤토리 장착 화면
    }

    public class Game
    {
        /* 게임 기본 세팅 */
        GameMode mode = GameMode.Lobby;
        MessageType msgtype = MessageType.Normal;

        Warrior player = new Warrior(1, "Newbie", 10, 5, 100, 150000);
        Store store = new Store();
        Inventory inventory = new Inventory();
        Utility utility = Utility.Instance;

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
                case GameMode.SellItem:
                    ProcessSellItem();
                    break;
                case GameMode.GoShelter:
                    ProcessGoShelter();
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
            Console.WriteLine("4.휴식하기");

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
                case 4:
                    msgtype = MessageType.Normal;
                    mode = GameMode.GoShelter;
                    break;
                default:
                    msgtype = MessageType.WrongInput;
                    break;
            }
        }

        public void ProcessShowState()
        {
            //mode = player.ProcessShowState(player, ref msgtype);
            mode = player.ProcessShowState(ref msgtype);
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
        public void ProcessSellItem()
        {
            mode = store.ProcessSellItem(player, ref msgtype, inventory);
        }
        public void ProcessGoShelter()
        {
            mode = player.ProcessGoShelter(ref msgtype);
        }
    }
}
