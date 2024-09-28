namespace TextRPG
{
    internal class Store
    {
        Utility utility = Utility.Instance;
        public List<Item> shoppingList = new List<Item>(capacity: 6);  // 상점에 판매할 아이템 목록

        // 상점 생성자 - 초기 아이템 목록 설정
        public Store()
        {
            // 방어 아이템
            DefenseItem item = new DefenseItem("수련자 갑옷", 5, "수련에 도움을 주는 갑옷입니다.", 1000);
            DefenseItem item1 = new DefenseItem("무쇠갑옷", 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000);
            DefenseItem item2 = new DefenseItem("스파르타의 갑옷", 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500);
            DefenseItem item6 = new DefenseItem("갑갑옷", 1, "갑옷이 아니라 그냥 갑갑한 옷입니다.", 5);

            // 공격 아이템
            AttackItem item3 = new AttackItem("낡은 검", 2, "쉽게 볼 수 있는 낡은 검 입니다.", 600);
            AttackItem item4 = new AttackItem("청동 도끼", 5, "어디선가 사용됐던거 같은 도끼입니다.", 1500);
            AttackItem item5 = new AttackItem("스파르타의 창", 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 3000);
            AttackItem item7 = new AttackItem("컴활", 10, "컴퓨터가 대신 에임을 맞춰주는 활입니다.", 4000);

            // 아이템들을 상점 목록에 추가
            shoppingList.Add(item);
            shoppingList.Add(item1);
            shoppingList.Add(item2);
            shoppingList.Add(item6);
            shoppingList.Add(item3);
            shoppingList.Add(item4);
            shoppingList.Add(item5);
            shoppingList.Add(item7);
        }

        // 상점에서 아이템 목록을 보여주는 함수
        public GameMode ProcessGoStore(int gold, Game gameInstance, ref MessageType msgtype)
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("아이템을 사고 팔 수 있는 상점입니다.\n");
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{gold} G");

            // 아이템 목록 출력
            utility.PrintItemList(PrintType.StoreMain, shoppingList);

            Console.WriteLine("\n1. 아이템구매");
            Console.WriteLine("2. 아이템판매");
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
                case 2:
                    msgtype = MessageType.Normal;
                    return GameMode.SellItem;
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

            // 아이템 목록 출력
            utility.PrintItemList(PrintType.StoreBuy, shoppingList);

            Console.WriteLine("\n0. 나가기\n");

            // 사용자 입력에 따라 처리
            int input = utility.InputFromUser(ref msgtype);
            if (input == 0)
            {
                msgtype = MessageType.Normal;
                return GameMode.GoStore;
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

        // 아이템 판매 관리 하는 함수
        public GameMode ProcessSellItem(Player player, ref MessageType msgtype, Inventory inven)
        {
            Console.Clear();
            Console.WriteLine("상점 - 아이템 판매");
            Console.WriteLine("필요없는 아이템을 팔 수 있는 상점입니다.\n");
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{player.Gold} G");

            // 아이템 목록 출력
            utility.PrintItemList(PrintType.StoreSell, inven.inventory);

            Console.WriteLine("\n0. 나가기\n");

            // 사용자 입력에 따라 처리
            int input = utility.InputFromUser(ref msgtype);
            if (input == 0)
            {
                msgtype = MessageType.Normal;
                return GameMode.GoStore;
            }
            else if (input >= 1 && input <= inven.inventory.Count)
            {
                msgtype = CheckSelling(input, player, inven);  // 판매 확인
                return GameMode.SellItem;
            }
            else
            {
                msgtype = MessageType.WrongInput;  // 잘못된 입력 처리
                return GameMode.SellItem;
            }
        }

        // 아이템 구매 유효성 체크 및 구입 관리하는 함수
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

        // 아이템 판매 유효성 체크 및 판매 관리하는 함수
        MessageType CheckSelling(int num, Player player, Inventory inven)
        {
            Item selectedItem = inven.inventory[num - 1];

            // 판매 처리
            player.Gold += (int)(selectedItem.Price * 0.85f);   // 원가의 85%
            selectedItem.IsOwn = false;

            // 장착된 아이템인 경우
            if (selectedItem.IsSet)
            {
                selectedItem.IsSet = false; // 장착 해제

                // 공격 아이템일 경우 플레이어 공격력에 반영
                AttackItem attackItem = selectedItem as AttackItem;
                if (attackItem != null)
                {
                    player.Attack -= attackItem.Attack;
                    player.BuffedAttack -= attackItem.Attack;
                }

                // 방어 아이템일 경우 플레이어 방어력에 반영
                DefenseItem defenseItem = selectedItem as DefenseItem;
                if (defenseItem != null)
                {
                    player.Defense -= defenseItem.Defense;
                    player.BuffedDefense -= defenseItem.Defense;
                }
            }
            inven.RemoveInven(selectedItem);    // 인벤토리에서 아이템 삭제
            return MessageType.SaleSucceed;  // 판매 성공
        }
    }
}
