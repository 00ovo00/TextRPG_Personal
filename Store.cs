using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    internal class Store
    {
        Utility utility = new Utility();
        public List<Item> shoppingList = new List<Item>(capacity: 6);  // 상점에 판매할 아이템 목록

        // 상점 생성자 - 초기 아이템 목록 설정
        public Store()
        {
            // 방어 아이템
            DefenseItem item = new DefenseItem("수련자 갑옷", 5, "수련에 도움을 주는 갑옷입니다.", 1000);
            DefenseItem item1 = new DefenseItem("무쇠갑옷", 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000);
            DefenseItem item2 = new DefenseItem("스파르타의 갑옷", 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500);

            // 공격 아이템
            AttackItem item3 = new AttackItem("낡은 검", 2, "쉽게 볼 수 있는 낡은 검 입니다.", 600);
            AttackItem item4 = new AttackItem("청동 도끼", 5, "어디선가 사용됐던거 같은 도끼입니다.", 1500);
            AttackItem item5 = new AttackItem("스파르타의 창", 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 3000);

            // 아이템들을 상점 목록에 추가
            shoppingList.Add(item);
            shoppingList.Add(item1);
            shoppingList.Add(item2);
            shoppingList.Add(item3);
            shoppingList.Add(item4);
            shoppingList.Add(item5);
        }

        // 상점에서 아이템 목록을 보여주는 함수
        public GameMode ProcessGoStore(int gold, Game gameInstance, ref MessageType msgtype)
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{gold} G");
            Console.WriteLine("\n[아이템 목록]");

            // 상점 아이템 목록 출력
            foreach (Item item in shoppingList)
            {
                string itemType = string.Empty;
                string itemValue = string.Empty;

                // 공격 아이템인지 방어 아이템인지 확인 후 출력 형식 설정
                if (item is AttackItem attackItem)
                {
                    itemType = "공격력";
                    itemValue = $"+{attackItem.Attack}";
                }
                else if (item is DefenseItem defenseItem)
                {
                    itemType = "방어력";
                    itemValue = $"+{defenseItem.Defense}";
                }

                // 아이템 정보를 정렬하여 출력
                string nameColumn = utility.PadRightWithVisualWidth(item.Name, 15);
                string typeColumn = utility.PadRightWithVisualWidth(itemType, 4);
                string valueColumn = utility.PadRightWithVisualWidth(itemValue, 4);
                string descriptionColumn = utility.PadRightWithVisualWidth(item.Description, 50);

                Console.Write(
                    $"- {nameColumn} | " +
                    $"{typeColumn} {valueColumn} | " +
                    $"{descriptionColumn} | "
                );
                if (item.IsOwn)
                    Console.WriteLine("구매완료");  // 이미 구매한 경우
                else
                    Console.WriteLine($"{item.Price} G");  // 구매하지 않은 경우 가격 출력
            }

            Console.WriteLine("\n1. 아이템구매");
            Console.WriteLine("0. 나가기\n");

            // 사용자 입력 처리
            int input = utility.InputFromUser(ref msgtype);
            switch (input)
            {
                case 0:
                    msgtype = MessageType.Normal;
                    return GameMode.Lobby;
                case 1:
                    msgtype = MessageType.Normal;
                    return GameMode.BuyItem;
                default:
                    msgtype = MessageType.WrongInput;
                    return GameMode.GoStore;
            }
        }

        // 아이템 구매 관리 하는 함수
        public GameMode ProcessBuyItem(Player player, ref MessageType msgtype, Inventory inven)
        {
            Console.Clear();
            Console.WriteLine("상점 - 아이템 구매");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G");
            Console.WriteLine("\n[아이템 목록]");

            int idx = 1;
            foreach (Item item in shoppingList)
            {
                string itemType = string.Empty;
                string itemValue = string.Empty;

                // 공격 아이템인지 방어 아이템인지 확인 후 출력 형식 설정
                if (item is AttackItem attackItem)
                {
                    itemType = "공격력";
                    itemValue = $"+{attackItem.Attack}";
                }
                else if (item is DefenseItem defenseItem)
                {
                    itemType = "방어력";
                    itemValue = $"+{defenseItem.Defense}";
                }

                // 아이템 정보를 정렬하여 출력
                string nameColumn = utility.PadRightWithVisualWidth(item.Name, 15);
                string typeColumn = utility.PadRightWithVisualWidth(itemType, 4);
                string valueColumn = utility.PadRightWithVisualWidth(itemValue, 4);
                string descriptionColumn = utility.PadRightWithVisualWidth(item.Description, 50);

                Console.Write(
                    $"- {idx++} {nameColumn} | " +
                    $"{typeColumn} {valueColumn} | " +
                    $"{descriptionColumn} | "
                );
                if (item.IsOwn)
                    Console.WriteLine("구매완료");  // 이미 구매한 경우
                else
                    Console.WriteLine($"{item.Price} G");  // 구매하지 않은 경우 가격 출력
            }

            Console.WriteLine("\n0. 나가기\n");

            // 사용자 입력에 따라 처리
            int input = utility.InputFromUser(ref msgtype);
            if (input == 0)
            {
                msgtype = MessageType.Normal;
                return GameMode.Lobby;  // 나가기
            }
            else if (input >= 1 && input <= shoppingList.Count)
            {
                msgtype = CheckBuying(input, player, inven);  // 구매 확인
                return GameMode.BuyItem;
            }
            else
            {
                msgtype = MessageType.WrongInput;  // 잘못된 입력 처리
                return GameMode.BuyItem;
            }
        }

        // 아이템 구매 유효성 체크 및 구입
        MessageType CheckBuying(int num, Player player, Inventory inven)
        {
            Item selectedItem = shoppingList[num - 1];

            // 구입 요청 유효성 체크
            if (selectedItem.IsOwn)
            {
                return MessageType.AlreadyBought;  // 이미 구매한 아이템일 경우
            }
            if (selectedItem.Price > player.Gold)
            {
                return MessageType.LackGold;  // 골드가 부족한 경우
            }

            // 구매 처리
            player.Gold -= selectedItem.Price;
            selectedItem.IsOwn = true;
            inven.AddInven(selectedItem);
            return MessageType.PurchaseSucceed;  // 구매 성공
        }
    }
}
