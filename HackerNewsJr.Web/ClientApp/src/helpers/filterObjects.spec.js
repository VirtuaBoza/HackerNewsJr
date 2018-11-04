import filterObjects from './filterObjects';

const testObjs = [
  {
    first: 'Bob',
    last: 'Smith',
  },
  {
    first: 'Wilma',
    last: 'Brown',
  },
  {
    first: 'Bob',
    last: 'Brown',
  },
  {
    first: 'William',
    last: 'Porter',
  },
];

describe('filterObjects', () => {
  it('filters objects by searchString (case insensitive) contained in fields', () => {
    // Arrange
    // Act
    // Assert
    expect(filterObjects(testObjs, 'o', ['first', 'last']).length).toBe(4);
    expect(filterObjects(testObjs, 'b', ['first', 'last']).length).toBe(3);
    expect(filterObjects(testObjs, 'b', ['last']).length).toBe(2);
    expect(filterObjects(testObjs, 'wil', ['first']).length).toBe(2);
  });
});
