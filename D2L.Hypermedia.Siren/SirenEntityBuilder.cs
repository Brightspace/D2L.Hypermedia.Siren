using System;
using System.Collections.Generic;
using System.Dynamic;

namespace D2L.Hypermedia.Siren {
    public sealed class SirenEntityBuilder {

        private List<string> m_rel;
        private List<ISirenAction> m_actions;
        private List<ISirenLink> m_links;
        private List<ISirenEntity> m_entities;
        private IDictionary<string, object> m_properties;
        private List<string> m_class;

        public SirenEntityBuilder AddRel( string rel ) {
            if( m_rel == null ) {
                m_rel = new List<string>();
            }

            m_rel.Add( rel );
            return this;
        }

        public SirenEntityBuilder AddRel( IEnumerable<string> rels ) {
            if( m_rel == null ) {
                m_rel = new List<string>();
            }

            m_rel.AddRange( rels );
            return this;
        }


        public SirenEntityBuilder AddAction( ISirenAction action ) {
            if( m_actions == null ) {
                m_actions = new List<ISirenAction>();
            }

            m_actions.Add( action );
            return this;
        }

        public SirenEntityBuilder AddActions( IEnumerable<ISirenAction> actions ) {
            if( m_actions == null ) {
                m_actions = new List<ISirenAction>();
            }

            m_actions.AddRange( actions );
            return this;
        }


        public SirenEntityBuilder AddLink( ISirenLink link ) {
            if( m_links == null ) {
                m_links = new List<ISirenLink>();
            }

            m_links.Add( link );
            return this;
        }


        public SirenEntityBuilder AddLinks( IEnumerable<ISirenLink> links ) {
            if( m_links == null ) {
                m_links = new List<ISirenLink>();
            }

            m_links.AddRange( links );
            return this;
        }

        public SirenEntityBuilder AddEntity( ISirenEntity entity ) {
            if( m_entities == null ) {
                m_entities = new List<ISirenEntity>();
            }

            m_entities.Add( entity );
            return this;
        }

        public SirenEntityBuilder AddEntities( IEnumerable<ISirenEntity> entities ) {
            if( m_entities == null ) {
                m_entities = new List<ISirenEntity>();
            }

            m_entities.AddRange( entities );
            return this;
        }

        public SirenEntityBuilder AddProperty( string name, object value ) {
            if( m_properties == null ) {
                m_properties = new ExpandoObject();
            }

            m_properties.Add( name, value );
            return this;
        }

        public SirenEntityBuilder AddClass( string @class ) {
            if( m_class == null ) {
                m_class = new List<string>();
            }

            m_class.Add( @class );
            return this;
        }

        public SirenEntityBuilder AddClass( IEnumerable<string> classes ) {
            if( m_class == null ) {
                m_class = new List<string>();
            }

            m_class.AddRange( classes );
            return this;
        }

        public ISirenEntity Build(
            string title = null,
            Uri href = null,
            string type = null
        ) {
            ISirenEntity entity = new SirenEntity(
                rel: m_rel?.ToArray(),
                @class: m_class?.ToArray(),
                properties: m_properties,
                entities: m_entities,
                links: m_links,
                actions: m_actions,
                title: title,
                href: href,
                type: type
            );
            return entity;
        }

    }
}
