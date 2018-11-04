import React from 'react';
import Footer from './Footer';
import { mount } from 'enzyme';

describe('Footer search input', () => {
  it('Renders searchString from props', () => {
    // Arrange
    const testString = 'test string';

    // Act
    const wrapper = mount(
      <Footer searchString={testString} onChange={() => {}} />,
    );

    // Assert
    const input = wrapper.find('input');
    expect(input.props().value).toBe(testString);
  });

  it('Changing input value calls onChange', () => {
    // Arrange
    const mockHandler = jest.fn();
    const wrapper = mount(<Footer searchString="" onChange={mockHandler} />);
    const input = wrapper.find('input');

    // Act
    input.simulate('change');

    // Assert
    expect(mockHandler.mock.calls.length).toBe(1);
  });
});
