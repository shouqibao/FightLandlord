using System;
using System.Collections.Generic;
using System.Linq;

namespace BetGame.DDZ {

	internal class Utils {
		private static readonly string[] allpokertexts = "🃃,🃓,🂳,🂣,🃄,🃔,🂴,🂤,🃅,🃕,🂵,🂥,🃆,🃖,🂶,🂦,🃇,🃗,🂷,🂧,🃈,🃘,🂸,🂨,🃉,🃙,🂹,🂩,🃊,🃚,🂺,🂪,🃋,🃛,🂻,🂫,🃍,🃝,🂽,🂭,🃎,🃞,🂾,🂮,🃁,🃑,🂱,🂡,🃂,🃒,🂲,🂢,🃟,🂿,🂠".Split(',');

		/// <summary>
		/// 获取一副新牌
		/// </summary>
		/// <returns></returns>
		internal static List<int> GetNewPoker() {
			var list = new List<int>();
			for (int a = 0; a < 54; a++) list.Add(a);
			return list;
		}
		/// <summary>
		/// 判断是否连续
		/// </summary>
		/// <param name="ps">使用前请先排序</param>
		/// <returns></returns>
		internal static bool IsSeries(IEnumerable<int> poker) {
			if (poker == null || poker.Any() == false) return false;
			if (poker.Last() >= 15) return false;
			int pp = 255;
			foreach (var p in poker) {
				if (pp != 255 && (
					p - pp != 1
					)) return false;
				pp = p;
			}
			return true;
		}
		/// <summary>
		/// 获取扑克牌文本
		/// </summary>
		/// <param name="poker"></param>
		/// <returns></returns>
		internal static string[] GetPokerText(IEnumerable<int> poker) {
			var sb = new List<string>();
			foreach (var p in poker)
				sb.Add(allpokertexts[p < 0 || p > 53 ? allpokertexts.Length - 1 : p]);
			return sb.ToArray();
		}
		internal static HandPokerComplieResult GetHandPokerComplieResult(HandPokerType type, IEnumerable<GroupByPokerResult> gb) {
			if (type == HandPokerType.个 ||
				type == HandPokerType.对 ||
				type == HandPokerType.三条) {
				var pk = gb.First().poker.OrderByDescending(a => a).ToArray();
				return new HandPokerComplieResult { type = type, compareValue = gb.First().key, value = pk, text = GetPokerText(pk) };
			}
			if (type == HandPokerType.三条带一个) {
				var gb3 = gb.Where(a => a.count == 3).First();
				var gb1 = gb.Where(a => a.count == 1).First();
				var value = gb3.poker.OrderByDescending(a => a).Concat(gb1.poker).ToArray();
				return new HandPokerComplieResult { type = type, compareValue = gb3.key, value = value, text = GetPokerText(value) };
			}
			if (type == HandPokerType.三条带一对) {
				var gb3 = gb.Where(a => a.count == 3).First();
				var gb2 = gb.Where(a => a.count == 2).First();
				var value = gb3.poker.OrderByDescending(a => a).Concat(gb2.poker.OrderByDescending(a => a)).ToArray();
				return new HandPokerComplieResult { type = type, compareValue = gb3.key, value = value, text = GetPokerText(value) };
			}
			if (type == HandPokerType.顺子) {
				var gbs = gb.OrderBy(a => a.key);
				var value = gbs.Select(a => a.poker.First()).ToArray();
				return new HandPokerComplieResult { type = type, compareValue = gbs.Last().key, value = value, text = GetPokerText(value) };
			}
			if (type == HandPokerType.连对 ||
				type == HandPokerType.飞机) {
				var gbs = gb.OrderBy(a => a.key);
				var tmp = new List<int>();
				int cv = 0;
				foreach (var g in gb) {
					var gpk = g.poker.OrderByDescending(a => a);
					tmp.AddRange(gpk);
					cv = g.key;
				}
				var value = tmp.ToArray();
				return new HandPokerComplieResult { type = type, compareValue = cv, value = value, text = GetPokerText(value) };
			}
			if (type == HandPokerType.飞机带个) {
				var gb3 = gb.Where(a => a.count == 3).OrderBy(a => a.key);
				var gb1 = gb.Where(a => a.count == 1).OrderBy(a => a.key);
				var tmp3 = new List<int>();
				int cv = 0;
				foreach (var g in gb3) {
					var gpk = g.poker.OrderByDescending(a => a);
					tmp3.AddRange(gpk);
					cv = g.key;
				}
				var tmp1 = new List<int>();
				foreach (var g in gb1) tmp1.Add(g.poker.First());
				var value = tmp3.Concat(tmp1).ToArray();
				return new HandPokerComplieResult { type = type, compareValue = cv, value = value, text = GetPokerText(value) };
			}
			if (type == HandPokerType.飞机带队) {
				var gb3 = gb.Where(a => a.count == 3).OrderBy(a => a.key);
				var gb2 = gb.Where(a => a.count == 2).OrderBy(a => a.key);
				var tmp3 = new List<int>();
				int cv = 0;
				foreach (var g in gb3) {
					var gpk = g.poker.OrderByDescending(a => a);
					tmp3.AddRange(g.poker.OrderByDescending(a => a));
					cv = g.key;
				}
				var tmp2 = new List<int>();
				foreach (var g in gb2) tmp2.AddRange(g.poker.OrderByDescending(a => a));
				var value = tmp3.Concat(tmp2).ToArray();
				return new HandPokerComplieResult { type = type, compareValue = cv, value = value, text = GetPokerText(value) };
			}
			if (type == HandPokerType.炸带二个) {
				int cv = 0;
				var gb4 = gb.Where(a => a.count == 4);
				var gb4len2 = gb4.Any() == false;
				if (gb4len2) gb4 = gb.Where(a => a.key == 16 || a.key == 17);
				cv = gb4.First().key;
				var gb1 = gb4len2 ? gb.Where(a => a.count == 1 && a.key < 16) : gb.Where(a => a.count == 1);
				var tmp4 = new List<int>();
				foreach (var g in gb4) tmp4.AddRange(g.poker.OrderByDescending(a => a));
				var tmp1 = new List<int>();
				foreach (var g in gb1) tmp1.AddRange(g.poker);
				var value = tmp4.Concat(tmp1).ToArray();
				return new HandPokerComplieResult { type = type, compareValue = cv, value = value, text = GetPokerText(value) };
			}
			if (type == HandPokerType.炸带二对) {
				int cv = 0;
				var gb4 = gb.Where(a => a.count == 4);
				var gb4len2 = gb4.Any() == false;
				if (gb4len2) gb4 = gb.Where(a => a.key == 16 || a.key == 17);
				cv = gb4.First().key;
				var gb2 = gb4len2 ? gb.Where(a => a.count == 1 && a.key < 16) : gb.Where(a => a.count == 2);
				var tmp4 = new List<int>();
				foreach (var g in gb4) tmp4.AddRange(g.poker.OrderByDescending(a => a));
				var tmp2 = new List<int>();
				foreach (var g in gb2) tmp2.AddRange(g.poker.OrderByDescending(a => a));
				var value = tmp4.Concat(tmp2).ToArray();
				return new HandPokerComplieResult { type = type, compareValue = gb4len2 ? tmp4.First() : tmp4.Last(), value = value, text = GetPokerText(value) };
			}
			if (type == HandPokerType.四条炸 ||
				type == HandPokerType.王炸) {
				var pk = gb.First().poker.OrderByDescending(a => a).ToArray();
				return new HandPokerComplieResult { type = type, compareValue = gb.First().key, value = pk, text = GetPokerText(pk) };
			}
			throw new ArgumentException("GetHandPokerComplieResult 参数错误");
		}
		/// <summary>
		/// 分组扑克牌
		/// </summary>
		/// <param name="poker"></param>
		/// <returns></returns>
		internal static IEnumerable<GroupByPokerResult> GroupByPoker(int[] poker) {
			if (poker == null || poker.Length == 0) return null;
			var dic = new Dictionary<int, GroupByPokerTmpResult>();
			for (var a = 0; a < poker.Length; a++) {
				int key = 0;
				if (poker[a] >= 0 && poker[a] < 52) key = (int)(poker[a] / 4 + 3);
				else if (poker[a] == 52) key = 16;
				else if (poker[a] == 53) key = 17;
				if (key == 0) throw new ArgumentException("poker 参数值错误");

				if (dic.ContainsKey(key) == false) dic.Add(key, new GroupByPokerTmpResult());
				dic[key].count++;
				dic[key].poker.Add(poker[a]);
			}
			return dic.Select(a => new GroupByPokerResult { key = a.Key, count = a.Value.count, poker = a.Value.poker }).OrderByDescending(a => a.count);
		}
		internal class GroupByPokerResult {
			internal int key { get; set; }
			internal int count { get; set; }
			internal IEnumerable<int> poker { get; set; }
		}
		class GroupByPokerTmpResult {
			internal int count { get; set; } = 0;
			internal List<int> poker { get; set; } = new List<int>();
		}
	}

}
