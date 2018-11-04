const fetchJson = async url => {
  const response = await fetch(url);
  if (response.ok) return await response.json();
  throw response;
};

export default fetchJson;
