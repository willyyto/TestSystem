import React from 'react';

interface LayoutProps {
}

const Layouts: React.FC<LayoutProps> = (props: React.PropsWithChildren<LayoutProps>
) => {

    return (
        <>
            {props.children}
        </>
    );
};

export default Layouts;
