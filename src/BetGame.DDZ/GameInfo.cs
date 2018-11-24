using System.Collections.Generic;
using System.Linq;

namespace BetGame.DDZ {
	public class GameInfo {
		/// <summary>
		/// 打多大（普通基数）结算：multiple * (multipleAddition + Bong)
		/// </summary>
		public decimal multiple { get; set; }
		/// <summary>
		/// 附加倍数，抢地主环节
		/// </summary>
		public decimal multipleAddition { get; set; }
		/// <summary>
		/// 设定最大附加倍数
		/// </summary>
		public decimal multipleAdditionMax { get; set; }
		/// <summary>
		/// 炸弹次数
		/// </summary>
		public decimal bong { get; set; }
		/// <summary>
		/// 游戏玩家
		/// </summary>
		public List<GamePlayer> players { get; set; }
		/// <summary>
		/// 轮到哪位玩家操作
		/// </summary>
		public int playerIndex { get; set; }
		/// <summary>
		/// 底牌
		/// </summary>
		public int[] dipai { get; set; }
		public string[] dipaiText => Utils.GetPokerText(this.dipai);
		/// <summary>
		/// 出牌历史
		/// </summary>
		public List<HandPokerInfo> chupai { get; set; }
		/// <summary>
		/// 当前游戏阶段
		/// </summary>
		public GameStage stage { get; set; }
	}

	public enum GameStage { 未开始, 叫地主, 斗地主, 游戏结束 }

	public class GamePlayer {
		/// <summary>
		/// 玩家
		/// </summary>
		public string id { get; set; }
		/// <summary>
		/// 玩家手上的牌
		/// </summary>
		public List<int> poker { get; set; }
		public string[] pokerText => Utils.GetPokerText(this.poker);
		/// <summary>
		/// 玩家最初的牌
		/// </summary>
		public List<int> pokerInit { get; set; }
		/// <summary>
		/// 玩家角色
		/// </summary>
		public GamePlayerRole role { get; set; }
	}

	public enum GamePlayerRole { 未知, 地主, 农民 }
}
