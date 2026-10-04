// The twelve sections of "With Love From My Kitchen", in book order.
export const SECTIONS = [
  { slug: 'appetizers', name: 'Appetizers, Beverages & Party Foods', blurb: 'Dips, punch and party snacks' },
  { slug: 'breads', name: 'Breads, Preserves, Jellies & Pickles', blurb: 'Quick breads, rolls and jams' },
  { slug: 'salads-soups', name: 'Salads, Dressings, Soups & Sandwiches', blurb: 'Cold salads and warm soups' },
  { slug: 'eggs-cheese', name: 'Eggs & Cheese', blurb: 'Brunch and baked eggs' },
  { slug: 'vegetables', name: 'Vegetables & Vegetable Snacks', blurb: 'Casseroles and potatoes' },
  { slug: 'pasta-grains', name: 'Pasta & Grains', blurb: 'Noodles, rice and lasagna' },
  { slug: 'poultry-seafood', name: 'Poultry, Fish & Seafoods', blurb: 'Chicken dinners and fish' },
  { slug: 'meats', name: 'Meats, Gravies & Meat Sauces', blurb: 'Beef, stews and sloppy joes' },
  { slug: 'desserts', name: 'Desserts, Fruits & Ices', blurb: 'Puddings and homemade ice cream' },
  { slug: 'cakes', name: 'Cakes, Frostings & Fillings', blurb: 'Layer cakes and frostings' },
  { slug: 'pies-cookies', name: 'Pies, Cookies, Candies & Pastries', blurb: 'Cookies, fudge and pies' },
  { slug: 'miscellaneous', name: 'Miscellaneous Extras, Secrets & Hints', blurb: 'Cookies and extras added later' },
] as const;

export type SectionSlug = (typeof SECTIONS)[number]['slug'];
export const SECTION_SLUGS = SECTIONS.map((s) => s.slug) as [SectionSlug, ...SectionSlug[]];
export const sectionBySlug = (slug: string) => SECTIONS.find((s) => s.slug === slug)!;
