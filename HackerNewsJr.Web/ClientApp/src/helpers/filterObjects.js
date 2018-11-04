const filterObjects = (objects, searchString, fields) => {
  searchString = searchString.toLowerCase();

  return objects.filter(story => {
    let found;
    fields.forEach(field => {
      const value = story[field].toLowerCase();
      if (value.includes(searchString)) {
        found = true;
      }
    });
    return found;
  });
};

export default filterObjects;
