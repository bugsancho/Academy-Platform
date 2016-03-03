namespace AcademyPlatform.Web.Umbraco.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Models.Payments;
    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Payments;

    using AutoMapper;

    using global::Umbraco.Web.Mvc;

    [EnsurePublishedContentRequest(2055)]
    public class SubscriptionsAdminController : SurfaceController
    {
        private readonly IRepository<CourseSubscription> _subscriptions;

        private readonly ISubscriptionsService _subscriptionsService;

        public SubscriptionsAdminController(IRepository<CourseSubscription> subscriptions, ISubscriptionsService subscriptionsService)
        {
            _subscriptions = subscriptions;
            _subscriptionsService = subscriptionsService;
        }

        public ActionResult Index()
        {
            List<CourseSubscription> awaitingPaymentSubscriptions =
                _subscriptions
                .All()
                .Where(x => x.Status == SubscriptionStatus.AwaitingPayment)
                .Include(x => x.User)
                .Include(x => x.Course)
                .ToList();

            return View(awaitingPaymentSubscriptions);
        }

        [HttpGet]
        public ActionResult AddPayment(int subscriptionId)
        {
            CourseSubscription subscription = _subscriptions.AllIncluding(x => x.Course).FirstOrDefault(x => x.Id == subscriptionId);
            if (subscription == null)
            {
                return HttpNotFound();
            }

            ViewBag.SubscriptionName = subscription.Course.Title;

            return View(new PaymentEditViewModel { SubscriptionId = subscriptionId });
        }

        [HttpPost]
        public ActionResult AddPayment(PaymentEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return AddPayment(viewModel.SubscriptionId);
            }

            Payment payment = Mapper.Map<Payment>(viewModel);
            _subscriptionsService.AddPayment(payment);

            return RedirectToAction(nameof(Index));
        }
    }
}