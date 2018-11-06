import React from 'react';
import StoryList from './StoryList';
import { shallow } from 'enzyme';

describe('StoryList', () => {
  it('Renders "a thing" for each story', () => {
    // Arrange
    const stories = [{ id: 1, title: 'title' }, { id: 2, title: 'title' }];

    // Act
    const wrapper = shallow(
      <StoryList
        stories={stories}
        searchString=""
        page={1}
        onMoreClick={() => {}}
      />,
    );

    // Assert
    expect(wrapper.find('.athing').length).toBe(2);
  });

  it('Has a "more" button when there are more than 30 stories', () => {
    // Arrange
    const stories = [];
    for (var i = 0; i < 31; i++) {
      stories.push({ id: i, title: 'title' });
    }

    // Act
    const wrapper = shallow(
      <StoryList
        stories={stories}
        searchString=""
        page={1}
        onMoreClick={() => {}}
      />,
    );

    // Assert
    expect(wrapper.find('.morelink').length).toBe(1);
  });

  it('Does NOT have a "more" button when there are 30 or less stories', () => {
    // Arrange
    const stories = [];
    for (var i = 0; i < 30; i++) {
      stories.push({ id: i, title: 'title' });
    }

    // Act
    const wrapper = shallow(
      <StoryList
        stories={stories}
        searchString=""
        page={1}
        onMoreClick={() => {}}
      />,
    );

    // Assert
    expect(wrapper.find('.morelink').length).toBe(0);
  });

  it('Calls click handler when the More button is clicked', () => {
    // Arrange
    const stories = [];
    for (var i = 0; i < 31; i++) {
      stories.push({ id: i, title: 'title' });
    }
    const handleClick = jest.fn();
    const wrapper = shallow(
      <StoryList
        stories={stories}
        searchString=""
        page={1}
        onMoreClick={handleClick}
      />,
    );
    wrapper.setProps({ page: 1 });

    // Act
    wrapper.find('.morelink').simulate('click');

    // Assert
    expect(handleClick.mock.calls.length).toBe(1);
  });
});
