using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BetGame.DDZ;

namespace BetGame.DDZ.WebHost.Controllers
{
	[Route("ddz"), ServiceFilter(typeof(CustomExceptionFilter))]
	public class DDZGamePlayController : Controller
    {
		GamePlay DDZCreate(string[] playerIds, decimal multiple) {
			var ddz = GamePlay.Create(playerIds, multiple, 3);
			return ddz;
		}
		GamePlay DDZGet(string id) {
			var ddz = GamePlay.GetById(id);
			return ddz;
		}
		void DDZOnShuffle(string id, GameInfo data) {

		}
		void DDZOnNextSelect(string id, GameInfo data) {

		}
		void DDZOnNextPlay(string id, GameInfo data) {

		}
		void DDZOnGameOver(string id, GameInfo data) {

		}

		[HttpPost("StartAndShuffle")]
		public APIReturn 开始并洗牌([FromForm] decimal multiple, [FromForm] string[] playerIds) {
			var ddz = DDZCreate(playerIds, multiple);
			ddz.Shuffle();
			return APIReturn.成功.SetData("id", ddz.Id, "data", ddz.Data);
		}

		[HttpPost("SelectLandlord")]
		public APIReturn 叫地主([FromForm] string id, [FromForm] string playerId, [FromForm] decimal multiple) {
			var ddz = DDZGet(id);
			ddz.SelectLandlord(playerId, multiple);
			return APIReturn.成功.SetData("id", ddz.Id, "data", ddz.Data);
		}
		[HttpPost("SelectFarmer")]
		public APIReturn 不叫([FromForm] string id, [FromForm] string playerId) {
			var ddz = DDZGet(id);
			ddz.SelectFarmer(playerId);
			return APIReturn.成功.SetData("id", ddz.Id, "data", ddz.Data);
		}

		[HttpPost("Play")]
		public APIReturn 出牌([FromForm] string id, [FromForm] string playerId, [FromForm] int[] poker) {
			var ddz = DDZGet(id);
			ddz.Play(playerId, poker);
			return APIReturn.成功.SetData("id", ddz.Id, "data", ddz.Data);
		}
		[HttpPost("Pass")]
		public APIReturn 不要([FromForm] string id, [FromForm] string playerId) {
			var ddz = DDZGet(id);
			ddz.Pass(playerId);
			return APIReturn.成功.SetData("id", ddz.Id, "data", ddz.Data);
		}
	}
}
