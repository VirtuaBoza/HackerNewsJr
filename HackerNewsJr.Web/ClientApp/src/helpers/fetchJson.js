const fetchJson = url => {
  return fetch(url).then(response => {
    if (response.ok) return Promise.resolve(response.json());
    return Promise.reject(new Error(response.statusText));
  });
};

export default fetchJson;
