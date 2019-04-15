using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Writers.DAL;
using Writers.Models;
using Writers.ViewModels;
using X.PagedList;

namespace Writers.Controllers
{
    public class PersonController : Controller
    {
        private WritersContext db = new WritersContext();

        // GET: Person
        public ActionResult Index(string searchString, string sortOrder, int? personsOnPage, int? page)
        {
            var persons = db.Persons.OrderBy(p => p.FullName);

            switch (sortOrder)
            {
                case "fullNameAsc":
                    sortOrder = "Full Name Ascending";
                    break;
                case "fullNameDesc":
                    sortOrder = "Full Name Descending";
                    break;
                case "countryAsc":
                    sortOrder = "Country Ascending";
                    break;
                case "countryDesc":
                    sortOrder = "Country Descending";
                    break;
                case "centuryAsc":
                    sortOrder = "Century Ascending";
                    break;
                case "centuryDesc":
                    sortOrder = "Century Descending";
                    break;
                default:
                    sortOrder = "Full Name Ascending";
                    break;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                persons = db.Persons.Where(p => p.FullName.Contains(searchString)).OrderBy(p => p.FullName);
                ViewBag.CurrentFilter = searchString;
                ViewBag.CurrentSearchText = "Person full name matching " + "\"" + searchString + "\" " + "(Sorted by " + sortOrder + ")";

                if (!String.IsNullOrEmpty(sortOrder))
                {
                    switch (sortOrder)
                    {
                        case "Full Name Ascending":
                            persons = persons.OrderBy(p => p.FullName);
                            break;
                        case "Full Name Descending":
                            persons = persons.OrderByDescending(p => p.FullName);
                            break;
                        case "Country Ascending":
                            persons = persons.OrderBy(p => p.Country);
                            break;
                        case "Country Descending":
                            persons = persons.OrderByDescending(p => p.Country);
                            break;
                        case "Century Ascending":
                            persons = persons.OrderBy(p => p.Century);
                            break;
                        case "Century Descending":
                            persons = persons.OrderByDescending(p => p.Century);
                            break;
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(sortOrder))
                {
                    switch (sortOrder)
                    {
                        case "Full Name Ascending":
                            persons = db.Persons.OrderBy(p => p.FullName);
                            break;
                        case "Full Name Descending":
                            persons = db.Persons.OrderByDescending(p => p.FullName);
                            break;
                        case "Country Ascending":
                            persons = db.Persons.OrderBy(p => p.Country);
                            break;
                        case "Country Descending":
                            persons = db.Persons.OrderByDescending(p => p.Country);
                            break;
                        case "Century Ascending":
                            persons = db.Persons.OrderBy(p => p.Century);
                            break;
                        case "Century Descending":
                            persons = db.Persons.OrderByDescending(p => p.Century);
                            break;
                    }
                }
            }

            var pageSize = personsOnPage ?? persons.Count();
            var pageNumber = page ?? 1;

            PersonsListViewModel model = new PersonsListViewModel()
            {
                PersonsListInfo = new PersonsListInfo()
                {
                    SearchString = searchString,
                    TotalItemsFound = persons.Count(),
                    ItemsOnPage = pageSize,
                    CurrentSortOrder = sortOrder
                },
                PagedList = persons.ToPagedList(pageNumber, pageSize)
            };

            return View(model);
        }

        // GET: Person/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }

            List<int> paragraphsCountList;
            PersonsDetailsViewModel model = new PersonsDetailsViewModel()
            {
                PersonsDetailsInfo = new PersonsDetailsInfo()
                {
                    BiographyTitles = ExtractBiographyTitles(person),
                    BiographyParagraphs = ExtractBiographyParagraphs(person, out paragraphsCountList),
                    BiographyParagraphsCount = paragraphsCountList
                },
                Person = person
            };

            return View(model);
        }

        List<string> ExtractBiographyTitles(Person person)
        {
            List<string> BiographyTitles = new List<string>();

            if (!String.IsNullOrEmpty(person.Biography) && person.Biography.Contains('\r'))
            {
                int startIndex = person.Biography.IndexOf('\r');
                int stopIndex = person.Biography.IndexOf('\r');

                BiographyTitles.Add(person.Biography.Substring(0, stopIndex));

                for (int i = startIndex + 2; i < person.Biography.Length; i++)
                {
                    if(person.Biography[i] == '\r' && person.Biography[i + 1] == '\n' && person.Biography[i + 2] == '\r' && person.Biography[i + 3] == '\n')
                    {
                        startIndex = i + 4;
                        stopIndex = i + 5;
                        while (person.Biography[stopIndex]  != '\r')
                            stopIndex++;
                        BiographyTitles.Add(person.Biography.Substring(startIndex, stopIndex - startIndex));
                        i = stopIndex;
                    }
                }
            }
            return BiographyTitles;
        }

        List<string> ExtractBiographyParagraphs(Person person, out List<int> paragraphsCountList)
        {
            List<string> BiographyParagraphs = new List<string>();

            paragraphsCountList = new List<int>();
            int paragraphsCounter = 0;

            if (!String.IsNullOrEmpty(person.Biography) && person.Biography.Contains('\r'))
            {
                int startIndex = person.Biography.IndexOf('\r');
                int stopIndex = person.Biography.IndexOf('\r');

                for(int i = startIndex + 2; i< person.Biography.Length; i++)
                {
                    if(person.Biography[i] == '\r' && person.Biography[i + 1] == '\n')
                    {
                        if(person.Biography[i + 2] != '\r' && person.Biography[i + 3] != '\n')
                        {
                            BiographyParagraphs.Add(person.Biography.Substring(startIndex + 2, i - (startIndex + 2)));
                            paragraphsCounter++;

                            startIndex = i + 2;
                            stopIndex = i + 3;
                        }
                        else if(person.Biography[i + 2] == '\r' && person.Biography[i + 3] == '\n')
                        {
                            BiographyParagraphs.Add(person.Biography.Substring(startIndex, i - startIndex));
                            paragraphsCountList.Add(paragraphsCounter + 1);
                            paragraphsCounter = 0;

                            startIndex = i + 4;
                            stopIndex = i + 5;
                            break;
                        }
                    }
                }

                for (int i = startIndex; i < person.Biography.Length; i++)
                {
                    if( i < person.Biography.Length - 1)
                    {
                        if (person.Biography[i] == '\r' && person.Biography[i + 1] == '\n')
                        {
                            if (person.Biography[i + 2] != '\r' && person.Biography[i + 3] != '\n')
                            {
                                startIndex = i + 2;
                                stopIndex = i + 3;
                                while (person.Biography[stopIndex] != '\r')
                                {
                                    if (stopIndex < person.Biography.Length - 1)
                                        stopIndex++;
                                    else
                                    {
                                        stopIndex++;
                                        break;
                                    }
                                }
                                BiographyParagraphs.Add(person.Biography.Substring(startIndex, stopIndex - startIndex));
                                paragraphsCounter++;

                                i = stopIndex - 1;
                            }
                            else if(person.Biography[i + 2] == '\r' && person.Biography[i + 3] == '\n')
                            {
                                startIndex = i + 4;
                                stopIndex = i + 5;
                                while (person.Biography[stopIndex] != '\r')
                                {
                                    if (stopIndex < person.Biography.Length - 1)
                                        stopIndex++;
                                    else
                                    {
                                        stopIndex++;
                                        break;
                                    }
                                }
                                BiographyParagraphs.Add(person.Biography.Substring(startIndex, stopIndex - startIndex));
                                paragraphsCountList.Add(paragraphsCounter);
                                paragraphsCounter = 0;

                                i = stopIndex - 1;
                            }   
                        }
                    }
                    else
                    {
                        BiographyParagraphs.Add(person.Biography.Substring(startIndex, stopIndex - startIndex));
                    }
                }
                paragraphsCountList.Add(paragraphsCounter);
            }
            return BiographyParagraphs;
        }

        // GET: Person/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Person/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FullName,Country,Century,Biography")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Persons.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(person);
        }

        // GET: Person/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: Person/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(string id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var personToUpdate = db.Persons.Find(id);

            if (TryUpdateModel(personToUpdate, "", new string[] { "Country", "Century", "Biography" }))
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DataException /*ex*/)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            return View(personToUpdate);
        }

        // GET: Person/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Person person = db.Persons.Find(id);
            db.Persons.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
