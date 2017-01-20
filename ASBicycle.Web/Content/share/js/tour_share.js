//此段代码用来适配各种手机屏幕
		$(window).bind('resize load', function() {
			$("body").css("zoom", $(window).width() / 750);
			$("body").css("display", "block");
		});
		
//		$.ajax({
//			type: "post",
//			url: "",
//			async: true,
//			dataType: "json",
//			success: function(data) {
//				
//				$(".year").html();
//				$(".month").html();
//				$(".day").html();
//				$(".area").html();
//				$(".times").html();
//				$(".head_img img").attr("src","")
//			},
//			error: function() {
//			    alert("网络错误,请刷新重试!")
//			    $("#box").empty()
//			}
//		});
		
		function ge_time_format(timestamp){
			if(timestamp) {
				var date = new Date(timestamp);
			} else {
				var date = new Date();
			}
			Y = date.getFullYear(),
			m = date.getMonth() + 1,
			d = date.getDate(),
			H = date.getHours(),
			i = date.getMinutes(),
			s = date.getSeconds();
			if(m < 10) {
				m = '0' + m;
			}
			if(d < 10) {
				d = '0' + d;
			}
			if(H < 10) {
				H = '0' + H;
			}
			if(i < 10) {
				i = '0' + i;
			}
			if(s < 10) {
				s = '0' + s;
			}
			var t = Y + '-' + m + '-' + d + ' ' + H + ':' + i + ':' + s;
			return t;
		}