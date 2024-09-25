using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    // 플레이어 클래스(기본)
    class Player
    {
        Utility utility = new Utility();

        // 필드
        private int _level;
        private string _name;
        private float _attack;
        private float _defense;
        private float _buffedAttack;
        private float _buffedDefense;
        private float _hp;
        private int _gold;

        // 프로퍼티
        public int Level { get; set; }
        public string Name { get; set; }
        public float Attack { get; set; }
        public float Defense { get; set; }
        public float BuffedAttack
        {
            get { return _buffedAttack; }
            set
            {
                if (value >= 0)
                    _buffedAttack = value;  // 버프 공격력은 0 이상일 때만 설정 가능
            }
        }
        public float BuffedDefense
        {
            get { return _buffedDefense; }
            set
            {
                if (value >= 0)
                    _buffedDefense = value;  // 버프 방어력은 0 이상일 때만 설정 가능
            }
        }
        public float Hp { get; set; }
        public int Gold { get; set; }

        // 플레이어 생성자 - 초기 상태 설정
        protected Player(int level, string name, float attack, float defense, float hp, int gold)
        {
            Level = level;
            Name = name;
            Attack = attack;
            Defense = defense;
            BuffedAttack = 0;
            BuffedDefense = 0;
            Hp = hp;
            Gold = gold;
        }


        // 플레이어의 상태를 출력하는 함수
        public GameMode ProcessShowState(Player player, ref MessageType msgtype)
        {
            Console.Clear();

            string job = null;
            if (player is Warrior)
                job = "전사";

            // 플레이어 상태 출력
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

            Console.WriteLine($"Lv. {player.Level.ToString("D2")}");
            Console.WriteLine(player.Name + $" ( {job} )");

            // 버프가 있을 경우 해당 값을 추가해서 출력
            if (player.BuffedAttack > 0)
                Console.WriteLine($"공격력 : {player.Attack - player.BuffedAttack} (+{player.BuffedAttack})");
            else
                Console.WriteLine($"공격력 : {player.Attack}");

            if (player.BuffedDefense > 0)
                Console.WriteLine($"방어력 : {player.Defense - player.BuffedDefense} (+{player.BuffedDefense})");
            else
                Console.WriteLine($"방어력 : {player.Defense}");

            Console.WriteLine($"체  력 : {player.Hp}");
            Console.WriteLine($" Gold  : {player.Gold}");

            Console.WriteLine("\n0. 나가기\n");

            // 사용자 입력 처리
            int input = utility.InputFromUser(ref msgtype);
            switch (input)
            {
                case 0:
                    msgtype = MessageType.Normal;
                    return GameMode.Lobby;
                default:
                    msgtype = MessageType.WrongInput;
                    return GameMode.ShowState;
            }
        }
        // 플레이어가 휴식할 수 있도록 관리하는 함수
        public GameMode ProcessGoShelter(Player player, ref MessageType msgtype)
        {
            int restPrice = 500;
            Console.Clear();
            Console.WriteLine("휴식하기");
            Console.WriteLine($"{restPrice} G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.Gold} G)");
            Console.WriteLine("\n1. 휴식하기");
            Console.WriteLine("0. 나가기\n");

            // 사용자 입력 처리
            int input = utility.InputFromUser(ref msgtype);
            switch (input)
            {
                case 0:
                    msgtype = MessageType.Normal;
                    return GameMode.Lobby;
                case 1:
                    msgtype = ProcessRest(player, restPrice);
                    return GameMode.GoShelter;
                default:
                    msgtype = MessageType.WrongInput;
                    return GameMode.GoShelter;
            }
        }
        // 휴식 요청의 유효성 체크 및 휴식 실행하는 함수
        MessageType ProcessRest(Player player, int price)
        {
            // 휴식 유효성 체크
            if (player.Hp >= 100)   // 휴식 가능한 상태인지 확인
                return MessageType.RestFailed;
            if (player.Gold < price)    // 골드가 충분한지 확인
                return MessageType.LackGold;
            // 골드 차감하고 체력 회복, 최대 회복체력 100으로 제한
            else
            {
                player.Gold -= price;
                player.Hp += 100;
                if (player.Hp >= 100)
                    player.Hp = 100;
                return MessageType.RestSucceed; // 휴식 완료
            }
        }
    }
    // 전사 클래스 (플레이어 클래스를 상속)
    class Warrior : Player
    {
        public Warrior(int level, string name, float attack, float defense, float hp, int gold)
            : base(level, name, attack, defense, hp, gold)
        {
        }
    }
}
