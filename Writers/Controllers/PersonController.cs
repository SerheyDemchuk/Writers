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
using Writers.Helpers;

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

            var res = persons.ToList();

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

            //List<int> paragraphsCountList;

            PersonsDetailsViewModel model = new PersonsDetailsViewModel()
            {
                PersonsDetailsInfo = new PersonsDetailsInfo()
                {
                    //BiographyTitles = ExtractBiographyTitles(person),
                    //BiographyParagraphs = ExtractBiographyParagraphs(person, out paragraphsCountList),
                    //BiographyParagraphsCount = paragraphsCountList,
                    BiographyDataNode = ExtractBiography(person)

                    //BiographyDataNode = new BiographyDataNode
                    //{
                    //    Data = "Foo",
                    //    Nodes = new[] {
                    //         new BiographyDataNode
                    //         {
                    //             Data = "Bar",
                    //             Nodes = new [] {
                    //                 new BiographyDataNode
                    //                 {
                    //                     Data = "Bing"
                    //                 },
                    //                 new BiographyDataNode
                    //                 {
                    //                     Data = "Bang"
                    //                 }
                    //             }
                    //         },
                    //         new BiographyDataNode
                    //         {
                    //             Data = "Bat"
                    //         },
                    //     },

                    //                },
                },
                Person = person
            };

            return View(model);
        }

        // Parse biography string from database
        // Extract headers and paragraphs 
        BiographyDataNode ExtractBiography(Person person)
        {

            BiographyDataNode BiographyData = new BiographyDataNode();

            //List<string> Paragraphs = new List<string>();

            //List<string> BiographyTitles = new List<string>();


            if (!String.IsNullOrEmpty(person.Biography) && person.Biography.Contains('\r'))
            {
                List<BiographyDataNode> secondLevelNodes = new List<BiographyDataNode>();
                List<BiographyDataNode> thirdLevelNodes = new List<BiographyDataNode>();

                string firstLevelTitle = "";
                List<string> secondLevelTitles = new List<string>();
                List<string> thirdLevelTitles = new List<string>();

                List<string> zeroLevelParagraphs = new List<string>();
                List<string> firstLevelParagraphs = new List<string>();
                List<string> secondLevelParagraphs = new List<string>();
                List<string> thirdLevelParagraphs = new List<string>();

                List<List<string>> FullZeroLevelParagraphs = new List<List<string>>();

                List<List<string>> FullFirstLevelParagraphs = new List<List<string>>();

                List<List<string>> FullSecondLevelParagraphs = new List<List<string>>();

                List<List<string>> FullThirdLevelParagraphs = new List<List<string>>();

                int firstLevelParagraphsCounter = 0;
                int secondLevelParagraphsCounter = 0;
                int thirdLevelParagraphsCounter = 0;

                int startIndex = 0;
                int stopIndex = 1;

                // Find and prepare images
                FindImages(person);

                // Find and prepare links
                FindLinks(person);

                // Find and prepare lists
                FindLists(person);

                // Find References section
                BiographyData.Reference = FindReferencesSection(person);

                // Extract summary if it exists
                for (int i = 0; i < person.Biography.Length - 1; i++)
                {
                    if (person.Biography[i] != '^')
                        continue;
                    else
                    {
                        zeroLevelParagraphs.Add(person.Biography.Substring(startIndex, i - 2));
                        FullZeroLevelParagraphs = CompleteParagraphs(zeroLevelParagraphs);

                        if (FullZeroLevelParagraphs.Count != 0)
                            BiographyData.Paragraph = FullZeroLevelParagraphs[0];

                        break;
                    }
                }

                    // Extract titles and paragraphs
                    for (int i = startIndex; i < person.Biography.Length - 1; i++)
                    {
                        if (person.Biography[i] == '^')
                        {

                            startIndex = i + 1;
                            stopIndex = i + 2;
                            while (person.Biography[stopIndex] != '\r')
                                stopIndex++;

                            //Extracting first level title
                            firstLevelTitle = person.Biography.Substring(startIndex, stopIndex - startIndex);
                            i = stopIndex;

                            //Searching for first level paragraphs and second level nodes
                            for (int j = stopIndex; j < person.Biography.Length - 1; j++)
                            {
                                //End of current first level node or end of biography
                                if ((person.Biography[j] == '\r' && person.Biography[j + 1] == '\n' && person.Biography[j + 2] == '\r' && person.Biography[j + 3] == '\n' && person.Biography[j + 4] == '^') || (j == person.Biography.Length - 5))
                                {
                                    if (firstLevelParagraphsCounter != 0 && firstLevelParagraphs.Count == 0)
                                        firstLevelParagraphs.Add(person.Biography.Substring(i + 2, j - i));

                                    i = j - 1;

                                    if (firstLevelParagraphs.Count != 0)
                                    {
                                        FullFirstLevelParagraphs = CompleteParagraphs(firstLevelParagraphs);
                                    }

                                    FullSecondLevelParagraphs = CompleteParagraphs(secondLevelParagraphs);


                                    for (int c = 0; c < secondLevelTitles.Count; c++)
                                    {
                                        secondLevelNodes.Add(new BiographyDataNode { Title = secondLevelTitles[c], Paragraph = new List<string>(FullSecondLevelParagraphs[c]) });
                                    }


                                    BiographyData.Nodes.Add(new BiographyDataNode()
                                    {
                                        Title = firstLevelTitle,
                                        Paragraph = FullFirstLevelParagraphs.Count != 0 ? FullFirstLevelParagraphs[0] : new List<string>(),
                                        Nodes = new List<BiographyDataNode>(secondLevelNodes)
                                    });

                                    firstLevelParagraphs.Clear();
                                    secondLevelParagraphs.Clear();
                                    secondLevelNodes.Clear();
                                    secondLevelTitles.Clear();
                                    FullFirstLevelParagraphs.Clear();

                                    break;
                                }

                                //Second level node
                                else if (person.Biography[j + 2] == '^' && person.Biography[j + 3] == '^')
                                {
                                    if (firstLevelParagraphsCounter != 0)
                                    {
                                        firstLevelParagraphs.Add(person.Biography.Substring(i + 2, j - i));
                                        firstLevelParagraphsCounter = 0;
                                    }

                                    startIndex = j + 4;
                                    stopIndex = j + 5;
                                    while (person.Biography[stopIndex] != '\r')
                                        stopIndex++;

                                    //Adding current second level title to the list
                                    secondLevelTitles.Add(person.Biography.Substring(startIndex, stopIndex - startIndex));
                                    j = stopIndex;

                                    //Searching for second and third level nodes 
                                    for (int z = stopIndex; z < person.Biography.Length - 1; z++)
                                    {
                                        //End of current first level node or end of biography
                                        if ((person.Biography[z] == '\r' && person.Biography[z + 1] == '\n' && person.Biography[z + 2] == '\r' && person.Biography[z + 3] == '\n' && person.Biography[z + 4] == '^') || (z == person.Biography.Length - 5))
                                        {
                                            if (secondLevelParagraphsCounter != 0 && thirdLevelTitles.Count == 0)
                                            {
                                                secondLevelParagraphs.Add(person.Biography.Substring(j + 2, z - j));
                                                secondLevelParagraphsCounter = 0;
                                            }
                                            else if (thirdLevelTitles.Count != 0)
                                            {
                                                thirdLevelParagraphs.Add(person.Biography.Substring(j + 2, z - j));
                                                thirdLevelParagraphsCounter = 0;

                                                if (secondLevelParagraphs.Count != 0)
                                                {
                                                    FullSecondLevelParagraphs = CompleteParagraphs(secondLevelParagraphs);
                                                }

                                                FullThirdLevelParagraphs = CompleteParagraphs(thirdLevelParagraphs);

                                                for (int c = 0; c < thirdLevelTitles.Count; c++)
                                                {
                                                    thirdLevelNodes.Add(new BiographyDataNode { Title = thirdLevelTitles[c], Paragraph = new List<string>(FullThirdLevelParagraphs[c]) });
                                                }

                                                foreach (string secondLevelTitle in secondLevelTitles)
                                                {
                                                    secondLevelNodes.Add(new BiographyDataNode
                                                    {
                                                        Title = secondLevelTitle,
                                                        Paragraph = FullSecondLevelParagraphs[0],
                                                        Nodes = new List<BiographyDataNode>(thirdLevelNodes)
                                                    });
                                                }

                                                secondLevelTitles.Clear();
                                                thirdLevelTitles.Clear();
                                                secondLevelParagraphs.Clear();
                                                thirdLevelParagraphs.Clear();
                                                FullThirdLevelParagraphs.Clear();
                                            }

                                            j = z - 1;
                                            break;
                                        }

                                        //Second level node
                                        else if (person.Biography[z + 2] == '^' && person.Biography[z + 3] == '^' && person.Biography[z + 4] != '^')
                                        {

                                            if (secondLevelParagraphsCounter != 0 && thirdLevelTitles.Count == 0)
                                            {
                                                secondLevelParagraphs.Add(person.Biography.Substring(j + 2, z - j));
                                                secondLevelParagraphsCounter = 0;
                                            }

                                            if (thirdLevelTitles.Count != 0)
                                            {
                                                thirdLevelParagraphs.Add(person.Biography.Substring(j + 2, z - j));
                                                thirdLevelParagraphsCounter = 0;
                                            }

                                            j = z - 1;

                                            if (thirdLevelTitles.Count != 0)
                                            {

                                                if (secondLevelParagraphs.Count != 0)
                                                {
                                                    FullSecondLevelParagraphs = CompleteParagraphs(secondLevelParagraphs);
                                                }

                                                FullThirdLevelParagraphs = CompleteParagraphs(thirdLevelParagraphs);

                                                for (int c = 0; c < thirdLevelTitles.Count; c++)
                                                {
                                                    thirdLevelNodes.Add(new BiographyDataNode { Title = thirdLevelTitles[c], Paragraph = new List<string>(FullThirdLevelParagraphs[c]) });
                                                }

                                                foreach (string secondLevelTitle in secondLevelTitles)
                                                {
                                                    secondLevelNodes.Add(new BiographyDataNode
                                                    {
                                                        Title = secondLevelTitle,
                                                        Paragraph = FullSecondLevelParagraphs[0],
                                                        Nodes = new List<BiographyDataNode>(thirdLevelNodes)
                                                    });
                                                }

                                                secondLevelTitles.Clear();
                                                thirdLevelTitles.Clear();
                                                secondLevelParagraphs.Clear();
                                                thirdLevelParagraphs.Clear();
                                                FullThirdLevelParagraphs.Clear();
                                                thirdLevelNodes.Clear();

                                                break;
                                            }
                                            else break;

                                        }

                                        //Third level node
                                        else if (person.Biography[z + 2] == '^' && person.Biography[z + 3] == '^' && person.Biography[z + 4] == '^')
                                        {
                                            if (secondLevelParagraphsCounter != 0)
                                            {
                                                secondLevelParagraphs.Add(person.Biography.Substring(j + 2, z - j));
                                                secondLevelParagraphsCounter = 0;
                                            }

                                            if (thirdLevelTitles.Count != 0)
                                            {
                                                thirdLevelParagraphs.Add(person.Biography.Substring(j + 2, z - j));
                                                thirdLevelParagraphsCounter = 0;
                                            }

                                            startIndex = z + 5;
                                            stopIndex = z + 6;
                                            while (person.Biography[stopIndex] != '\r')
                                                stopIndex++;
                                            thirdLevelTitles.Add(person.Biography.Substring(startIndex, stopIndex - startIndex));
                                            j = stopIndex;
                                            z = stopIndex;
                                            thirdLevelParagraphsCounter++;

                                            continue;
                                        }
                                        else
                                        {
                                            if (thirdLevelTitles.Count == 0)
                                                secondLevelParagraphsCounter++;
                                        }
                                    }
                                }

                                else
                                {
                                    firstLevelParagraphsCounter++;
                                }

                            }
                        }

                    }

                //Add title for table of contents
                BiographyData.Nodes.Add(new BiographyDataNode()
                {
                    Title = "References"
                });

            }

            return BiographyData;
        }

        //Extract references
        List<string> FindReferencesSection(Person person)
        {
            string refName = "^References";
            List<string> References = new List<string>();

            int referencesIndex = person.Biography.IndexOf(refName);
            if (referencesIndex != -1)
            {
                for (int i = referencesIndex + (refName.Length + 2); i < person.Biography.Length; i++)
                {
                    if (i == person.Biography.Length - 1)
                    {
                        References.Add(person.Biography.Substring(referencesIndex + (refName.Length + 2), i - (referencesIndex + (refName.Length + 1))));
                        person.Biography = person.Biography.Remove(referencesIndex);
                        break;
                    }

                }
            }

            return CompleteParagraphs(References)[0];

        }

        // Break paragraph assosiated with particular heading into paragraphs
        List<List<string>> CompleteParagraphs(List<string> paragraphsList)
        {
            List<List<string>> FullParagraphes = new List<List<string>>();
            int startIndex = 0;
            int stopIndex = 1;

            foreach (var paragraph in paragraphsList)
            {
                List<string> Paragraphes = new List<string>();
                for (int i = 0; i < paragraph.Length - 1; i++)
                {
                    if (paragraph[i] == '\r' || i == paragraph.Length - 2)
                    {
                        if (i == paragraph.Length - 2)
                        {

                            Paragraphes.Add(paragraph.Substring(startIndex, i - startIndex + 2));
                            startIndex = 0;
                            stopIndex = 1;
                            break;
                        }

                        Paragraphes.Add(paragraph.Substring(startIndex, i - startIndex));

                        startIndex = i + 2;
                        stopIndex = i + 2;
                        while (paragraph[stopIndex] != '\r')
                        {
                            if (stopIndex < paragraph.Length - 2)
                            {
                                stopIndex++;

                            }
                            else break;
                        }

                        if (stopIndex != paragraph.Length - 2)
                        {
                            Paragraphes.Add(paragraph.Substring(startIndex, stopIndex - startIndex + 2));
                            startIndex = stopIndex + 2;
                            i = stopIndex;
                        }
                        else
                        {
                            Paragraphes.Add(paragraph.Substring(startIndex, stopIndex - startIndex + 2));
                            break;
                        }
                    }
                }

                FullParagraphes.Add(Paragraphes);
                startIndex = 0;
                stopIndex = 0;
            }
            return FullParagraphes;
        }

        // Find and complete images
        void FindImages(Person person)
        {
            //string imgTag;
            string description;
            //int stopIndex = 0;

            for (int it = 0; it < person.Biography.Length - 1; it++)
            {
                //Div block with image
                if(person.Biography[it] == '~' && person.Biography[it + 1] == 'd' && person.Biography[it + 2] == 'i' && person.Biography[it + 3] == 'v')
                {
                    person.Biography = person.Biography.Insert(it + 1, "<");
                    person.Biography = person.Biography.Remove(it, 1);

                    for (int i = it; i < person.Biography.Length - 1; i++)
                    {
                        //Start of img tag
                        if (person.Biography[i] == '~' && person.Biography[i + 1] == 'i' && person.Biography[i + 2] == 'm' && person.Biography[i + 3] == 'g')
                        {
                            person.Biography = person.Biography.Remove(i, 1);
                            person.Biography = person.Biography.Insert(i - 1, ">");

                            person.Biography = person.Biography.Insert(i + 1, "<");
                            
                            for (int j = i; j < person.Biography.Length - 1; j++)
                            {
                                //End of img tag
                                if (person.Biography[j] == '~' && person.Biography[j + 1] == 'i' && person.Biography[j + 2] == 'm' && person.Biography[j + 3] == 'g')
                                {
                                    person.Biography = person.Biography.Insert(j, "/");
                                    person.Biography = person.Biography.Insert(j + 1, ">");
                                    person.Biography = person.Biography.Remove(j + 2, 4);

                                    //Extracting img description
                                    for (int z = j + 2; z < person.Biography.Length; z++)
                                    {
                                        //End of img description
                                        if (person.Biography[z] == '~' && person.Biography[z + 1] == 'd')
                                        {
                                            description = person.Biography.Substring(j + 2, z - (j + 2));
                                            person.Biography = person.Biography.Remove(j + 2, z + 2 - (j + 2));

                                            string taggedDescription = "<div class=\"card-body\">" + "<p class=\"card-text\">" + description + "</p>" + "</div>" + "</div>";

                                            person.Biography = person.Biography.Insert(z - description.Length, taggedDescription);
                                            //person.Biography = person.Biography.Insert(j + 2, "<div class=\"card card - details\" style=\"width: 18rem; \">");

                                            i = person.Biography.Length - 2;
                                            j = person.Biography.Length - 2;
                                            it = z;

                                            break;
                                        }
                                    }

                                }
                            }
                        }
                    }
                }

            }
        }

        void FindLinks(Person person)
        {
            for (int it = 0; it < person.Biography.Length - 1; it++)
            {
                //Ordered/Unordered list
                if (person.Biography[it] == '~' && person.Biography[it + 1] == 'a')
                {
                    person.Biography = person.Biography.Remove(it, 1);
                    person.Biography = person.Biography.Insert(it, "<");
                    
                    for (int i = it; i < person.Biography.Length - 1; i++)
                    {
                        //First list item
                        if (person.Biography[i] == '~' && person.Biography[i + 1] == 't')
                        {
                            person.Biography = person.Biography.Remove(i - 1, 4);
                            person.Biography = person.Biography.Insert(i - 1, ">");

                            for (int j = i; j < person.Biography.Length - 1; j++)
                            {
                                if (person.Biography[j] == '~' && person.Biography[j + 1] == 'a')
                                {
                                    person.Biography = person.Biography.Remove(j - 1, 2);
                                    person.Biography = person.Biography.Insert(j - 1, "</");
                                    person.Biography = person.Biography.Insert(j + 2, ">");

                                    it = j;
                                    i = person.Biography.Length - 2;
                                    break;
                                }

                            }
                        }
                    }
                }

            }
        }

        void FindLists(Person person)
        {
            for (int it = 0; it < person.Biography.Length - 1; it++)
            {
                //Ordered/Unordered list
                if ((person.Biography[it] == '~' && person.Biography[it + 1] == 'u' && person.Biography[it + 2] == 'l') || (person.Biography[it] == '~' && person.Biography[it + 1] == 'o' && person.Biography[it + 2] == 'l'))
                {
                    person.Biography = person.Biography.Remove(it, 1);
                    person.Biography = person.Biography.Insert(it, "<");
                    person.Biography = person.Biography.Insert(it + 3, ">");

                    for (int i = it; i < person.Biography.Length - 1; i++)
                    {

                        //First list item
                        if (person.Biography[i] == '~' && person.Biography[i + 1] == 'l' && person.Biography[i + 2] == 'i')
                        {
                            person.Biography = person.Biography.Remove(i - 1, 2);

                            person.Biography = person.Biography.Insert(i - 1, "<");
                            person.Biography = person.Biography.Insert(i + 2, ">");

                            for (int j = i; j < person.Biography.Length - 1; j++)
                            {
                                if ((person.Biography[j] == '~' && person.Biography[j + 1] == 'u' && person.Biography[j + 2] == 'l') || (person.Biography[j] == '~' && person.Biography[j + 1] == 'o' && person.Biography[j + 2] == 'l'))
                                {
                                    person.Biography = person.Biography.Remove(j, 1);
                                    person.Biography = person.Biography.Insert(j, "</");
                                    person.Biography = person.Biography.Insert(j + 4, ">");
                                    person.Biography = person.Biography.Insert(j, "</li>");

                                    it = j;
                                    i = person.Biography.Length - 2;
                                    break;
                                }

                                //Next list item
                                if (person.Biography[j] == '~' && person.Biography[j + 1] == 'l' && person.Biography[j + 2] == 'i')
                                {
                                    person.Biography = person.Biography.Remove(j, 1);
                                    person.Biography = person.Biography.Insert(j, "<");
                                    person.Biography = person.Biography.Insert(j + 3, ">");
                                    person.Biography = person.Biography.Insert(j, "</li>");

                                    i = j;

                                }
                            }
                        }
                    }
                }

            }
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
                    return RedirectToAction("Details", new { id= id});
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







//            BiographyData.Nodes.Add(new BiographyDataNode()
//            {
//                Data = firstLevelTitle,
//                Nodes = new List<BiographyDataNode>()
//{
//    new BiographyDataNode { Data = secondLevelTitle,
//    Nodes = new List<BiographyDataNode>()
//{
//    new BiographyDataNode {Data = thirdLevelTitle }
//}}

//}
//            });



//List<string> ExtractBiographyParagraphs(Person person, out List<int> paragraphsCountList)
//{
//    List<string> BiographyParagraphs = new List<string>();

//    paragraphsCountList = new List<int>();
//    int paragraphsCounter = 0;

//    if (!String.IsNullOrEmpty(person.Biography) && person.Biography.Contains('\r'))
//    {
//        int startIndex = person.Biography.IndexOf('\r');
//        int stopIndex = person.Biography.IndexOf('\r');

//        for(int i = startIndex + 2; i< person.Biography.Length; i++)
//        {
//            if(person.Biography[i] == '\r' && person.Biography[i + 1] == '\n')
//            {
//                if(person.Biography[i + 2] != '\r' && person.Biography[i + 3] != '\n')
//                {
//                    BiographyParagraphs.Add(person.Biography.Substring(startIndex + 2, i - (startIndex + 2)));
//                    paragraphsCounter++;

//                    startIndex = i + 2;
//                    stopIndex = i + 3;
//                }
//                else if(person.Biography[i + 2] == '\r' && person.Biography[i + 3] == '\n')
//                {
//                    BiographyParagraphs.Add(person.Biography.Substring(startIndex, i - startIndex));
//                    paragraphsCountList.Add(paragraphsCounter + 1);
//                    paragraphsCounter = 0;

//                    startIndex = i + 4;
//                    stopIndex = i + 5;
//                    break;
//                }
//            }
//        }

//        for (int i = startIndex; i < person.Biography.Length; i++)
//        {
//            if( i < person.Biography.Length - 1)
//            {
//                if (person.Biography[i] == '\r' && person.Biography[i + 1] == '\n')
//                {
//                    if (person.Biography[i + 2] != '\r' && person.Biography[i + 3] != '\n')
//                    {
//                        startIndex = i + 2;
//                        stopIndex = i + 3;
//                        while (person.Biography[stopIndex] != '\r')
//                        {
//                            if (stopIndex < person.Biography.Length - 1)
//                                stopIndex++;
//                            else
//                            {
//                                stopIndex++;
//                                break;
//                            }
//                        }
//                        BiographyParagraphs.Add(person.Biography.Substring(startIndex, stopIndex - startIndex));
//                        paragraphsCounter++;

//                        i = stopIndex - 1;
//                    }
//                    else if(person.Biography[i + 2] == '\r' && person.Biography[i + 3] == '\n')
//                    {
//                        startIndex = i + 4;
//                        stopIndex = i + 5;
//                        while (person.Biography[stopIndex] != '\r')
//                        {
//                            if (stopIndex < person.Biography.Length - 1)
//                                stopIndex++;
//                            else
//                            {
//                                stopIndex++;
//                                break;
//                            }
//                        }
//                        BiographyParagraphs.Add(person.Biography.Substring(startIndex, stopIndex - startIndex));
//                        paragraphsCountList.Add(paragraphsCounter);
//                        paragraphsCounter = 0;

//                        i = stopIndex - 1;
//                    }   
//                }
//            }
//            else
//            {
//                BiographyParagraphs.Add(person.Biography.Substring(startIndex, stopIndex - startIndex));
//            }
//        }
//        paragraphsCountList.Add(paragraphsCounter);
//    }
//    return BiographyParagraphs;
//}

//IQueryable<Person> OrderByLastName(bool desc = false)
//{
//    var persons = db.Persons.Select(p => p);

//    foreach (var person in persons)
//    {
//        person.FullName = SwapNameWords(person.FullName);
//    }

//    return persons.OrderBy(p => p.FullName);

//}

//string SwapNameWords(string personFullName, bool swapBack = false)
//{
//    string fullName = "";
//    string lastName = "";

//    if (!swapBack)
//    {
//        lastName = personFullName.Substring(personFullName.LastIndexOf(" ") + 1);
//        fullName = personFullName.Remove(personFullName.LastIndexOf(" "));
//        fullName = lastName + " " + fullName;
//    }
//    else
//    {
//        lastName = personFullName.Substring(0, personFullName.IndexOf(" "));
//        fullName = personFullName.Remove(0, personFullName.IndexOf(" ") + 1);
//        fullName = fullName + " " + lastName;
//    }


//    return fullName;
//}