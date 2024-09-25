using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    class Inventory
    {
        Utility utility = new Utility();
        public List<Item> inventory = new List<Item>(capacity: 6);

        // 인벤토리 화면을 출력하는 함수
        public GameMode ProcessShowInven(ref MessageType msgtype)
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");

            // 아이템 출력
            utility.PrintItemList(PrintType.InvenMain, inventory);

            Console.WriteLine("\n1. 장착 관리");
            Console.WriteLine("0. 나가기\n");

            // 사용자 입력에 따라 장착 관리 화면 또는 로비로 이동
            int input = utility.InputFromUser(ref msgtype);
            switch (input)
            {
                case 0:
                    msgtype = MessageType.Normal;
                    return GameMode.Lobby;
                case 1:
                    msgtype = MessageType.Normal;
                    return GameMode.EquipItem;
                default:
                    msgtype = MessageType.WrongInput;
                    return GameMode.ShowInven;
            }

        }
        // 아이템 장착 관리 화면을 관리하는 함수
        public GameMode ProcessEquipItem(Player player, Game gameInstance, ref MessageType msgtype)
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");

            // 아이템 출력
            utility.PrintItemList(PrintType.InvenEquip, inventory);

            Console.WriteLine("\n0. 나가기\n");

            // 사용자 입력에 따라 아이템 장착/해제 또는 나가기 처리
            int input = utility.InputFromUser(ref msgtype);
            if (input == 0)
            {
                msgtype = MessageType.Normal;
                return GameMode.ShowInven;
            }
            else if (input >= 1 && input <= inventory.Count)
            {
                EquipItem(input - 1, player);  // 선택된 아이템 장착/해제
                return GameMode.EquipItem;
            }
            else
            {
                msgtype = MessageType.WrongInput;
                return GameMode.EquipItem;
            }
        }

        // 인벤토리에 아이템을 추가하는 함수
        public void AddInven(Item item)
        {
            inventory.Add(item);
        }
        // 인벤토리의 아이템을 삭제하는 함수
        public void RemoveInven(Item item)
        {
            inventory.Remove(item);
        }

        // 아이템을 장착 또는 해제하는 함수
        public void EquipItem(int num, Player player)
        {
            Item item = inventory[num];

            int sign;  // 장착이면 +1, 해제이면 -1
            if (item.IsSet) { sign = -1; }
            else { sign = 1; }
            item.IsSet = !item.IsSet;  // 아이템 장착 상태 토글

            // 공격 아이템일 경우 플레이어 공격력에 반영
            AttackItem attackItem = item as AttackItem;
            if (attackItem != null)
            {
                player.Attack += attackItem.Attack * sign;
                player.BuffedAttack += attackItem.Attack * sign;
            }

            // 방어 아이템일 경우 플레이어 방어력에 반영
            DefenseItem defenseItem = item as DefenseItem;
            if (defenseItem != null)
            {
                player.Defense += defenseItem.Defense * sign;
                player.BuffedDefense += defenseItem.Defense * sign;
            }
        }
    }
}
