import React from 'react';
import StoryList from './StoryList';
import { shallow } from 'enzyme';

describe('StoryList', () => {
  it('Renders "a thing" for each story', () => {
    // Arrange
    const stories = [{ id: 1, title: 'title' }, { id: 2, title: 'title' }];

    // Act
    const wrapper = shallow(<StoryList stories={stories} searchString="" />);

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
    const wrapper = shallow(<StoryList stories={stories} searchString="" />);

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
    const wrapper = shallow(<StoryList stories={stories} searchString="" />);

    // Assert
    expect(wrapper.find('.morelink').length).toBe(0);
  });

  it('Increments the page number in state when the More button is clicked', () => {
    // Arrange
    const stories = [];
    for (var i = 0; i < 31; i++) {
      stories.push({ id: i, title: 'title' });
    }
    const wrapper = shallow(<StoryList stories={stories} searchString="" />);
    wrapper.setState({ page: 1 });

    // Act
    wrapper.find('.morelink').simulate('click');

    // Assert
    expect(wrapper.state().page).toBe(2);
  });
});
