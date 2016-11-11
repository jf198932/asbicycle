using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Uow;
using ASBicycle.Track;
using ASBicycle.User;
using ASBicycle.Web.Models.Share;
using Castle.Core.Internal;

namespace ASBicycle.Web.Controllers.Share
{
    public class ShareController : ASBicycleControllerBase
    {
        private readonly ITrackReadRepository _trackReadRepository;
        private readonly IUserReadRepository _userReadRepository;

        public ShareController(ITrackReadRepository trackReadRepository, IUserReadRepository userReadRepository)
        {
            _trackReadRepository = trackReadRepository;
            _userReadRepository = userReadRepository;
        }

        // GET: Share
        [UnitOfWork]
        public virtual async Task<ActionResult> Index(string paydocno)
        {
            var model = new ShareModel();
            if (!paydocno.IsNullOrEmpty())
            {
                var tracks = await _trackReadRepository.GetAllListAsync(t => t.Pay_docno == paydocno);
                var track = tracks.FirstOrDefault();
                if (track != null)
                {
                    TimeSpan costtime = DateTime.Parse(track.End_time.ToString()) - DateTime.Parse(track.Start_time.ToString());
                    var ctm = (int)costtime.TotalMinutes;//去掉多余的零头
                    var min = 0;
                    var hh = Math.DivRem(ctm, 60, out min);

                    model.HH = hh;
                    model.MM = min;
                    model.Place = track.Bike.School.Name;

                    var users = await _userReadRepository.GetAllListAsync(t => t.Id == track.User_id);
                    var user = users.FirstOrDefault();
                    if (user != null)
                    {
                        model.HeadImg = user.HeadImg;
                    }
                }
            }
            return View(model);
        }
    }
}