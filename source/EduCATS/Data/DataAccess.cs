﻿using System.Collections.Generic;
using System.Threading.Tasks;
using EduCATS.Data.Caching;
using EduCATS.Data.Models;
using EduCATS.Data.Models.Calendar;
using EduCATS.Data.Models.News;
using EduCATS.Data.Models.Subjects;
using EduCATS.Data.Models.User;
using EduCATS.Helpers.Json;
using EduCATS.Networking.AppServices;
using Nyxbull.Plugins.CrossLocalization;
using Xamarin.Essentials;

namespace EduCATS.Data
{
	public static partial class DataAccess
	{
		const string profileInfoKey = "PROFILE_INFO_KEY";
		const string getNewsKey = "GET_NEWS_KEY";
		const string getProfileInfoSubjectKey = "GET_PROFILE_INFO_SUBJECT_KEY";
		const string getProfileInfoCalendarKey = "GET_PROFILE_INFO_CALENDAR_KEY";

		public static void ResetData()
		{
			DataCaching<object>.RemoveCache();
		}

		public async static Task<UserModel> Login(string username, string password)
		{
			if (!checkConnectionEstablished()) {
				return getErrorObject() as UserModel;
			}

			var response = await AppServices.Login(username, password);
			var dataAccess = new DataAccess<UserModel>();
			var user = dataAccess.GetAccess(response, isCaching: false);

			if (user == null) {
				return getErrorObject("login_error_text") as UserModel;
			}

			return user;
		}

		public async static Task<UserProfileModel> GetProfileInfo(string username)
		{
			if (!checkConnectionEstablished()) {
				var data = getDataFromCache(profileInfoKey);
				return JsonController<UserProfileModel>.ConvertJsonToObject(data);
			}

			var response = await AppServices.GetProfileInfo(username);
			var dataAccess = new DataAccess<UserProfileModel>();
			var userProfile = dataAccess.GetAccess(response, profileInfoKey);

			if (userProfile == null) {
				return getErrorObject("login_user_profile_error_text") as UserProfileModel;
			}

			return userProfile;
		}

		public async static Task<NewsModel> GetNews(string username)
		{
			if (!checkConnectionEstablished()) {
				var data = getDataFromCache(getNewsKey);
				var list = JsonController<List<NewsItemModel>>.ConvertJsonToObject(data);
				return new NewsModel {
					News = list
				};
			}

			var response = await AppServices.GetNews(username);
			var dataAccess = new DataAccess<List<NewsItemModel>>();
			var newsList = dataAccess.GetAccess(response, getNewsKey);

			if (newsList == null) {
				return getErrorObject("today_news_load_error_text") as NewsModel;
			}

			return new NewsModel {
				News = newsList
			};
		}

		public async static Task<SubjectModel> GetProfileInfoSubjects(string username)
		{
			if (!checkConnectionEstablished()) {
				var data = getDataFromCache(getProfileInfoSubjectKey);
				var list = JsonController<List<SubjectItemModel>>.ConvertJsonToObject(data);
				return new SubjectModel {
					SubjectsList = list
				};
			}

			var response = await AppServices.GetProfileInfoSubjects(username);
			var dataAccess = new DataAccess<List<SubjectItemModel>>();
			var subjectsList = dataAccess.GetAccess(response, getProfileInfoSubjectKey);

			if (subjectsList == null) {
				return getErrorObject("today_subjects_error_text") as SubjectModel;
			}

			return new SubjectModel {
				SubjectsList = subjectsList
			};
		}

		public async static Task<CalendarModel> GetProfileInfoCalendar(string username)
		{
			if (!checkConnectionEstablished()) {
				var data = getDataFromCache(getProfileInfoCalendarKey);
				return JsonController<CalendarModel>.ConvertJsonToObject(data);
			}

			var response = await AppServices.GetProfileInfoCalendar(username);
			var dataAccess = new DataAccess<CalendarModel>();
			var calendar = dataAccess.GetAccess(response, getProfileInfoCalendarKey);

			if (calendar == null) {
				return getErrorObject("today_calendar_error_text") as CalendarModel;
			}

			return calendar;
		}

		static bool checkConnectionEstablished()
		{
			return Connectivity.NetworkAccess == NetworkAccess.Internet;
		}

		static string getDataFromCache(string key)
		{
			return DataCaching<string>.Get(key);
		}

		static DataModel getErrorObject(string localizedString = null)
		{
			return new DataModel {
				IsError = true,
				ErrorMessage = CrossLocalization.Translate(localizedString ?? "common_connection_error_text")
			};
		}
	}
}