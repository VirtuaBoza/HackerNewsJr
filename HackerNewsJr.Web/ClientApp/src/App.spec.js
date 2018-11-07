import React from 'react';
import App from './App';
import { shallow } from 'enzyme';

describe('App', () => {
  beforeEach(() => {
    fetch.mockResponse(
      JSON.stringify([{ id: 1, title: 'title', by: 'author', url: 'url' }]),
    );
  });

  it('Renders loading message while loading', () => {
    // Arrange
    const wrapper = shallow(<App />);

    // Act
    wrapper.setState({ loading: true });

    // Assert
    expect(wrapper.find('#LoadingMessage').length).toBe(1);
    expect(wrapper.find('#LoadingFailMessage').length).toBe(0);
    expect(wrapper.find('#StoryList').length).toBe(0);
  });

  it('Renders error message when loadingError is set', () => {
    // Arrange
    fetch.mockResponse(JSON.stringify({}), { status: 500 });
    const wrapper = shallow(<App />);

    // Act
    wrapper.setState({ loading: false, loadingError: new Error() });

    // Assert
    expect(wrapper.find('#LoadingMessage').length).toBe(0);
    expect(wrapper.find('#LoadingFailMessage').length).toBe(1);
    expect(wrapper.find('#StoryList').length).toBe(0);
  });

  it('Renders StoryList when loading completes with stories', () => {
    // Arrange
    const wrapper = shallow(<App />);

    // Act
    wrapper.setState({ loading: false, stories: [], filteredStories: [] });

    // Assert
    expect(wrapper.find('#LoadingMessage').length).toBe(0);
    expect(wrapper.find('#LoadingFailMessage').length).toBe(0);
    expect(wrapper.find('#StoryList').length).toBe(1);
  });

  describe('loadNewStories', () => {
    it('sets loadingError in state on fail', () => {
      // Arrange
      fetch.mockResponse(JSON.stringify({}), { status: 500 });
      const wrapper = shallow(<App />);

      // Act
      return wrapper
        .instance()
        .loadNewStories()
        .then(() => {
          // Assert
          expect(wrapper.state().loadingError).toBeTruthy();
          expect(wrapper.state().loading).toBe(false);
        });
    });

    it('sets stories and filteredStories in state', () => {
      // Arrange
      const wrapper = shallow(<App />);
      expect(wrapper.state().stories.length).toBe(0);
      expect(wrapper.state().filteredStories.length).toBe(0);

      // Act
      return wrapper
        .instance()
        .loadNewStories()
        .then(() => {
          // Assert
          expect(wrapper.state().stories.length).toBeGreaterThan(0);
          expect(wrapper.state().filteredStories.length).toBeGreaterThan(0);
        });
    });
  });
});
