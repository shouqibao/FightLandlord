using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[JsonObject(MemberSerialization.OptIn)]
public partial class APIReturn : ContentResult {
	[JsonProperty("code")] public int Code { get; protected set; }
	[JsonProperty("message")] public string Message { get; protected set; }
	[JsonProperty("data")] public Hashtable Data { get; protected set; } = new Hashtable();
	[JsonProperty("success")] public bool Success { get { return this.Code == 0; } }

	public APIReturn() { }
	public APIReturn(int code) { this.SetCode(code); }
	public APIReturn(string message) { this.SetMessage(message); }
	public APIReturn(int code, string message, params object[] data) { this.SetCode(code).SetMessage(message).AppendData(data); }

	public APIReturn SetCode(int value) { this.Code = value; return this; }
	public APIReturn SetMessage(string value) { this.Message = value; return this; }
	public APIReturn SetData(params object[] value) {
		this.Data.Clear();
		return this.AppendData(value);
	}
	public APIReturn AppendData(params object[] value) {
		if (value == null || value.Length < 2 || value[0] == null) return this;
		for (int a = 0; a < value.Length; a += 2) {
			if (value[a] == null) continue;
			this.Data[value[a]] = a + 1 < value.Length ? value[a + 1] : null;
		}
		return this;
	}
	#region form 表单 target=iframe 提交回调处理
	private void Jsonp(ActionContext context) {
		string __callback = context.HttpContext.Request.HasFormContentType ? context.HttpContext.Request.Form["__callback"].ToString() : null;
		if (string.IsNullOrEmpty(__callback)) {
			this.ContentType = "text/json;charset=utf-8;";
			this.Content = JsonConvert.SerializeObject(this);
		} else {
			this.ContentType = "text/html;charset=utf-8";
			this.Content = $"<script>top.{__callback}({GlobalExtensions.Json(null, this)});</script>";
		}
	}
	public override void ExecuteResult(ActionContext context) {
		Jsonp(context);
		base.ExecuteResult(context);
	}
	public override Task ExecuteResultAsync(ActionContext context) {
		Jsonp(context);
		return base.ExecuteResultAsync(context);
	}
	#endregion

	public static APIReturn 成功 { get { return new APIReturn(0, "成功"); } }
	public static APIReturn 失败 { get { return new APIReturn(99, "失败"); } }
	public static APIReturn 记录不存在_或者没有权限 { get { return new APIReturn(98, "记录不存在，或者没有权限"); } }
	public static APIReturn 参数格式不正确 { get { return new APIReturn(97, "参数格式不正确"); } }
}
