import fetchJson from './fetchJson';

describe('fetchJson', () => {
  beforeEach(() => {
    fetch.mockResponse(JSON.stringify({ id: 1, title: 'title' }));
  });

  it(`fails with synchronous code`, () => {
    const responseJson = fetchJson('http://foo.bar');
    expect(responseJson).not.toHaveProperty('title', 'title');
  });

  it(`returns Json using promises`, () => {
    expect.assertions(1);
    return fetchJson('http://foo.bar').then(responseJson => {
      expect(responseJson).toHaveProperty('title', 'title');
    });
  });

  it(`returns Json async/await`, async () => {
    const responseJson = await fetchJson('http://foo.bar');
    expect(responseJson).toHaveProperty('title', 'title');
  });

  it('throws response on fail', async () => {
    fetch.mockResponse(JSON.stringify({}), { status: 500 });
    expect(fetchJson('http://foo.bar')).rejects.toBeInstanceOf(Response);
  });
});
