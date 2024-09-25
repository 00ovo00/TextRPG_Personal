using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    class Utility
    {
        // 사용자 입력을 받고 메시지 타입에 따라 적절한 메시지를 출력하는 함수
        public int InputFromUser(ref MessageType msgType)
        {
            Console.WriteLine();
            switch (msgType)
            {
                case MessageType.WrongInput:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
                case MessageType.AlreadyBought:
                    Console.WriteLine("이미 구매한 아이템입니다.");
                    break;
                case MessageType.PurchaseSucceed:
                    Console.WriteLine("구매를 완료했습니다. ");
                    break;
                case MessageType.SaleSucceed:
                    Console.WriteLine("판매를 완료했습니다. ");
                    break;
                case MessageType.LackGold:
                    Console.WriteLine("Gold 가 부족합니다.");
                    break;
                case MessageType.RestSucceed:
                    Console.WriteLine("휴식을 완료했습니다.");
                    break;
                case MessageType.RestFailed:
                    Console.WriteLine("체력이 충분합니다! 휴식할 수 없습니다.");
                    break;
            }
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            // 입력된 문자열을 숫자로 변환, 실패 시 -1 반환
            if (int.TryParse(Console.ReadLine(), out int result))
            {
                return result;
            }
            return -1;
        }
        // 아이템 목록을 출력하는 함수
        public void PrintItemList(PrintType type, List<Item> itemList)
        {
            Console.WriteLine("\n[아이템 목록]");

            int idx = 1;
            foreach (Item item in itemList)
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

                int nameWidth = 15;
                int typeWidth = 4;
                int descriptWidth = 50;

                // 아이템 정보를 정렬하여 출력
                string nameColumn = PadRightWithVisualWidth(item.Name, nameWidth);
                string typeColumn = PadRightWithVisualWidth(itemType, typeWidth);
                string valueColumn = PadRightWithVisualWidth(itemValue, typeWidth);
                string descriptionColumn = PadRightWithVisualWidth(item.Description, descriptWidth);

                if (type == PrintType.StoreBuy || type == PrintType.StoreSell)
                    Console.Write($"- {idx++} {nameColumn} | ");
                else if (type == PrintType.InvenMain  ||type == PrintType.InvenEquip)
                {
                    // 아이템이 장착 중이면 [E] 표시
                    string eFlag = "   ";
                    if (item.IsSet) { eFlag = "[E]"; }
                    Console.Write($"- {idx++} {eFlag} {nameColumn} |");
                }
                else
                    Console.Write($"- {nameColumn} | ");

                Console.Write(
                    $"{typeColumn} {valueColumn} | " +
                    $"{descriptionColumn} "
                );
                switch (type)
                {
                    case PrintType.StoreMain:
                    case PrintType.StoreBuy:
                        {
                            if (item.IsOwn)
                                Console.WriteLine(" | 구매완료");  // 이미 구매한 경우
                            else
                                Console.WriteLine($" | {item.Price} G");
                            break;
                        }
                    case PrintType.StoreSell:
                        Console.WriteLine($" | {(int)(item.Price * 0.85f)} G");
                        break;
                    case PrintType.InvenMain:
                    case PrintType.InvenEquip:
                        Console.WriteLine("");
                        break;
                }
            }
        }

        // 주어진 텍스트의 너비(한글과 영어를 구분)를 계산하는 함수
        public int AlignWidth(string text)
        {
            int width = 0;
            foreach (char c in text)
            {
                // 한글일 경우 너비를 2로, 영어일 경우 1로 계산
                // if (c >= 'ㄱ' && c <= 'ㅎ' || c >= '가' && c <= '힣')
                if (c >= 0x1100 && c <= 0x11FF || c >= 0xAC00 && c <= 0xD7A3)
                {
                    width += 2;
                }
                else
                {
                    width += 1;
                }
            }
            return width;
        }

        // 주어진 너비에 맞춰 텍스트를 오른쪽으로 패딩하는 함수
        public string PadRightWithVisualWidth(string text, int totalWidth)
        {
            int visualWidth = AlignWidth(text);  // 텍스트의 너비 계산
            int paddingNeeded = totalWidth - visualWidth;  // 필요한 패딩 계산
            return text + new string(' ', Math.Max(paddingNeeded, 0));  // 패딩을 추가하여 반환
        }
    }
}
