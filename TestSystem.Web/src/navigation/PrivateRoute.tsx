import React, {useContext} from 'react';
import {Navigate, RouteProps, useLocation} from 'react-router-dom';
import AuthContext from 'services/AuthContext';

interface PrivateRouteProps extends RouteProps {
    element: React.ReactElement;
}

const PrivateRoute: React.FC<PrivateRouteProps> = ({element, ...rest}) => {
    const {user} = useContext(AuthContext)!;
    const location = useLocation();

    return user ? (
        element
    ) : (
        <Navigate to="/login" state={{from: location}}/>
    );
};

export default PrivateRoute;
