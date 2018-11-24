using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BetGame.DDZ {
	public class GamePlayTest {

		[Fact]
		public void Create() {
			Dictionary<string, GameInfo> db = new Dictionary<string, GameInfo>();
			GamePlay.OnGetData = id => db.TryGetValue(id, out var tryout) ? tryout : null;
			GamePlay.OnSaveData = (id, d) => {
				db.TryAdd(id, d);
			};

			Assert.Throws<ArgumentException>(() => GamePlay.Create(null, 2, 5));
			Assert.Throws<ArgumentException>(() => GamePlay.Create(new[] { "玩家1", "玩家2", "玩家3", "玩家4" }, 2, 5));
			Assert.Throws<ArgumentException>(() => GamePlay.Create(new[] { "玩家1", "玩家2" }, 2, 5));

			var ddz = GamePlay.Create(new[] { "玩家1", "玩家2", "玩家3" }, 2, 5);
			var data = db[ddz.Id];
			//洗牌，发牌
			Assert.NotNull(ddz);
			Assert.NotNull(data);
			Assert.Equal(GameStage.未开始, data.stage);

			ddz.Shuffle();
			Assert.Equal(0, data.bong);
			Assert.Empty(data.chupai);
			Assert.Equal(3, data.dipai.Length);
			Assert.Equal(2, data.multiple);
			Assert.Equal(0, data.multipleAddition);
			Assert.Equal(5, data.multipleAdditionMax);
			Assert.Equal(3, data.players.Count);
			Assert.Equal(GamePlayerRole.未知, data.players[0].role);
			Assert.Equal(GamePlayerRole.未知, data.players[1].role);
			Assert.Equal(GamePlayerRole.未知, data.players[2].role);
			Assert.Equal(17, data.players[0].pokerInit.Count);
			Assert.Equal(17, data.players[1].pokerInit.Count);
			Assert.Equal(17, data.players[2].pokerInit.Count);
			Assert.Equal(17, data.players[0].poker.Count);
			Assert.Equal(17, data.players[1].poker.Count);
			Assert.Equal(17, data.players[2].poker.Count);
			Assert.Equal("玩家1", data.players[0].id);
			Assert.Equal("玩家2", data.players[1].id);
			Assert.Equal("玩家3", data.players[2].id);
			Assert.Equal(GameStage.叫地主, data.stage);

			//牌是否重复
			Assert.Equal(54, data.players[0].poker.Concat(data.players[1].poker).Concat(data.players[2].poker).Concat(data.dipai).Distinct().Count());

			//GetById
			Assert.Equal(GamePlay.GetById(ddz.Id).Id, ddz.Id);
			Assert.Throws<ArgumentException>(() => GamePlay.GetById(null));
			Assert.Throws<ArgumentException>(() => GamePlay.GetById(""));
			Assert.Throws<ArgumentException>(() => GamePlay.GetById("slkdjglkjsdg"));

			//抢地主
			Assert.Throws<ArgumentException>(() => ddz.SelectLandlord("玩家10", 1));
			Assert.Throws<ArgumentException>(() => ddz.SelectFarmer("玩家10"));
			Assert.Throws<ArgumentException>(() => ddz.SelectLandlord(data.players[Math.Min(data.playerIndex + 1, data.players.Count - 1)].id, 1));
			Assert.Throws<ArgumentException>(() => ddz.SelectFarmer(data.players[Math.Min(data.playerIndex + 1, data.players.Count - 1)].id));
			ddz.SelectLandlord(data.players[data.playerIndex].id, 1);
			Assert.Equal(GameStage.叫地主, data.stage);
			Assert.Throws<ArgumentException>(() => ddz.SelectLandlord(data.players[data.playerIndex].id, 1));
			Assert.Throws<ArgumentException>(() => ddz.SelectLandlord(data.players[data.playerIndex].id, 100));
			ddz.SelectLandlord(data.players[data.playerIndex].id, 2);
			Assert.Equal(GameStage.叫地主, data.stage);
			ddz.SelectLandlord(data.players[data.playerIndex].id, 3);
			Assert.Equal(GameStage.叫地主, data.stage);

			ddz.SelectFarmer(data.players[data.playerIndex].id);
			Assert.Equal(GameStage.叫地主, data.stage);
			Assert.Equal(2, data.players.Where(a => a.role == GamePlayerRole.未知).Count());

			//以封顶倍数抢得地主
			//ddz.SelectLandlord(data.players[data.playerIndex].player, 5);
			//两个农民都不抢，由报分的人抢得地主
			ddz.SelectFarmer(data.players[data.playerIndex].id);

			Assert.Equal(GameStage.斗地主, data.stage);
			Assert.Equal(GamePlayerRole.地主, data.players[data.playerIndex].role);
			Assert.Equal(2, data.players.Where(a => a.role == GamePlayerRole.农民).Count());


		}
	}
}
