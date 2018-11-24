using System;

namespace BetGame.DDZ {
	public class HandPokerInfo {
		/// <summary>
		/// 出牌时间
		/// </summary>
		public DateTime time { get; set; }
		/// <summary>
		/// 这手牌出自哪位玩家
		/// </summary>
		public int playerIndex { get; set; }
		/// <summary>
		/// 牌编译结果
		/// </summary>
		public HandPokerComplieResult result { get; set; }
	}

	public enum HandPokerType { 个, 对, 三条, 三条带一个, 三条带一对, 顺子, 连对, 飞机, 飞机带个, 飞机带队, 炸带二个, 炸带二对, 四条炸, 王炸 }

	public class HandPokerComplieResult {
		public HandPokerType type { get; set; }
		/// <summary>
		/// 相同类型比较大小
		/// </summary>
		public int compareValue { get; set; }
		/// <summary>
		/// 牌
		/// </summary>
		public int[] value { get; set; }
		/// <summary>
		/// 牌面字符串
		/// </summary>
		public string[] text { get; set; }
	}
}
