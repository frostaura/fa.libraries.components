/*
 * The below is to address the issue of any bars overlaying the UI and forcing it to be scrollable (Like Safari's tab bar).
 * 
 * Credit to: https://medium.com/@susiekim9/how-to-compensate-for-the-ios-viewport-unit-bug-46e78d54af0d
 */

window.resizeNavigationLayout = () => {
    const navLayoutElement = document.querySelector('fa-navigation-layout');

    navLayoutElement.style.height = window.innerHeight + 'px';
}

window.addEventListener('resize', window.resizeNavigationLayout);